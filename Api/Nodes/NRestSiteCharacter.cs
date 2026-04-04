using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;

namespace LittleWizard.Api.Nodes;

public partial class NRestSiteCharacter : Node2D
{
    private static readonly StringName V = new("v");

    private static readonly StringName S = new("s");

    private static readonly StringName Noise2Panning = new("Noise2Panning");

    private static readonly StringName Noise1Panning = new("Noise1Panning");

    private static readonly StringName GlobalOffset = new("GlobalOffset");

    private static readonly Vector2 MultiplayerConfirmationOffset = new(-25f, -123f);

    private static readonly Vector2 MultiplayerConfirmationFlipOffset = new(-155f, 0f);

    private static readonly string MultiplayerConfirmationScenePath =
        SceneHelper.GetScenePath("rest_site/rest_site_multiplayer_confirmation");

    private int _characterIndex;

    public required Control ControlRoot;

    private RestSiteOption? _hoveredRestSiteOption;

    public required Control LeftThoughtAnchor;

    private RestSiteOption? _restSiteOptionInThoughtBubble;

    public required Control RightThoughtAnchor;

    private Control? _selectedOptionConfirmation;

    private RestSiteOption? _selectingRestSiteOption;

    public required NSelectionReticle SelectionReticle;

    private CancellationTokenSource? _thoughtBubbleGoAwayCancellation;

    private NThoughtBubbleVfx? _thoughtBubbleVfx;

    public required Control Hitbox { get; set; }

    public required Player Player { get; set; }

    public static NRestSiteCharacter Create(Player player, int characterIndex)
    {
        var nRestSiteCharacter = PreloadManager.Cache.GetScene(player.Character.RestSiteAnimPath)
            .Instantiate<NRestSiteCharacter>();
        nRestSiteCharacter.Player = player;
        nRestSiteCharacter._characterIndex = characterIndex;
        return nRestSiteCharacter;
    }

    public override void _Ready()
    {
        ControlRoot = GetNode<Control>("ControlRoot");
        Hitbox = GetNode<Control>("%Hitbox");
        SelectionReticle = GetNode<NSelectionReticle>("%SelectionReticle");
        LeftThoughtAnchor = GetNode<Control>("%ThoughtBubbleLeft");
        RightThoughtAnchor = GetNode<Control>("%ThoughtBubbleRight");
        var animationName = Player.RunState.CurrentActIndex switch
        {
            0 => "overgrowth_loop",
            1 => "hive_loop",
            2 => "glory_loop",
            _ => throw new InvalidOperationException("Unexpected act")
        };
        foreach (var childSpineNode in GetChildSpineNodes())
        {
            var megaTrackEntry = new MegaSprite(childSpineNode).GetAnimationState().SetAnimation(animationName);
            megaTrackEntry?.SetTrackTime(megaTrackEntry.GetAnimationEnd() * Rng.Chaotic.NextFloat());
        }

        if (Player.Character is Necrobinder)
        {
            var node = GetNode<Sprite2D>("%NecroFire");
            var node2 = GetNode<Sprite2D>("%OstyFire");
            RandomizeFire((ShaderMaterial)node.Material);
            RandomizeFire((ShaderMaterial)node2.Material);
            if (_characterIndex >= 2)
            {
                var node3 = GetNode<Node2D>("Osty");
                var node4 = GetNode<Node2D>("OstyRightAnchor");
                node3.Position = node4.Position;
                MoveChild(node3, 0);
            }
        }

        Hitbox.Connect(Control.SignalName.FocusEntered, Callable.From(OnFocus));
        Hitbox.Connect(Control.SignalName.FocusExited, Callable.From(OnUnfocus));
        Hitbox.Connect(Control.SignalName.MouseEntered, Callable.From(OnFocus));
        Hitbox.Connect(Control.SignalName.MouseExited, Callable.From(OnUnfocus));
    }

    private static void RandomizeFire(ShaderMaterial mat)
    {
        mat.SetShaderParameter(GlobalOffset,
            new Vector2(Rng.Chaotic.NextFloat(-50f, 50f), Rng.Chaotic.NextFloat(-50f, 50f)));
        mat.SetShaderParameter(Noise1Panning,
            mat.GetShaderParameter(Noise1Panning).AsVector2() + new Vector2(Rng.Chaotic.NextFloat(-0.1f, 0.1f),
                Rng.Chaotic.NextFloat(-0.1f, 0.1f)));
        mat.SetShaderParameter(Noise2Panning,
            mat.GetShaderParameter(Noise2Panning).AsVector2() + new Vector2(Rng.Chaotic.NextFloat(-0.1f, 0.1f),
                Rng.Chaotic.NextFloat(-0.1f, 0.1f)));
    }

    private void OnFocus()
    {
        if (!NTargetManager.Instance.IsInSelection || !NTargetManager.Instance.AllowedToTargetNode(this)) return;
        NTargetManager.Instance.OnNodeHovered(this);
        SelectionReticle.OnSelect();
        NRun.Instance?.GlobalUi.MultiplayerPlayerContainer.HighlightPlayer(Player);
    }

    private void OnUnfocus()
    {
        if (NTargetManager.Instance.IsInSelection && NTargetManager.Instance.AllowedToTargetNode(this))
            NTargetManager.Instance.OnNodeUnhovered(this);
        Deselect();
    }

    private void Deselect()
    {
        if (SelectionReticle.IsSelected) SelectionReticle.OnDeselect();
        NRun.Instance?.GlobalUi.MultiplayerPlayerContainer.UnhighlightPlayer(Player);
    }

    private void FlipX()
    {
        Vector2 scale;
        foreach (var childSpineNode in GetChildSpineNodes())
        {
            scale = childSpineNode.Scale;
            scale.X = 0f - childSpineNode.Scale.X;
            childSpineNode.Scale = scale;
            scale = childSpineNode.Position;
            scale.X = 0f - childSpineNode.Position.X;
            childSpineNode.Position = scale;
        }

        var controlRoot = ControlRoot;
        scale = ControlRoot.Scale;
        scale.X = 0f - ControlRoot.Scale.X;
        controlRoot.Scale = scale;
    }

    private void HideFlameGlow()
    {
        foreach (var childSpineNode in GetChildSpineNodes())
        {
            var megaSprite = new MegaSprite(childSpineNode);
            if (megaSprite.HasAnimation("_tracks/light_off"))
                megaSprite.GetAnimationState().SetAnimation("_tracks/light_off", true, 1);
        }
    }

    public void ShowHoveredRestSiteOption(RestSiteOption? option)
    {
        _hoveredRestSiteOption = option;
        RefreshThoughtBubbleVfx();
    }

    public void SetSelectingRestSiteOption(RestSiteOption? option)
    {
        _selectingRestSiteOption = option;
        RefreshThoughtBubbleVfx();
    }

    private void RefreshThoughtBubbleVfx()
    {
        if (_selectedOptionConfirmation != null) return;
        var restSiteOption = _selectingRestSiteOption ?? _hoveredRestSiteOption;
        if (_restSiteOptionInThoughtBubble == restSiteOption) return;
        _restSiteOptionInThoughtBubble = restSiteOption;
        if (restSiteOption == null)
        {
            TaskHelper.RunSafely(RemoveThoughtBubbleAfterDelay());
            return;
        }

        _thoughtBubbleGoAwayCancellation?.Cancel();
        if (_thoughtBubbleVfx == null)
        {
            var characterIndex = _characterIndex;
            var flag = characterIndex is 0 or 3;
            _thoughtBubbleVfx = NThoughtBubbleVfx.Create(restSiteOption.Icon,
                !flag ? DialogueSide.Left : DialogueSide.Right, null);
            if (_thoughtBubbleVfx == null) return;
            var shaderMaterial = (ShaderMaterial)_thoughtBubbleVfx.GetNode<TextureRect>("%Image").Material;
            shaderMaterial.SetShaderParameter(S, 0.145f);
            shaderMaterial.SetShaderParameter(V, 0.85f);
            this.AddChildSafely(_thoughtBubbleVfx);
            _thoughtBubbleVfx.GlobalPosition = GetRestSiteOptionAnchor().GlobalPosition;
        }
        else
        {
            _thoughtBubbleVfx.SetTexture(restSiteOption.Icon);
        }
    }

    public void ShowSelectedRestSiteOption(RestSiteOption option)
    {
        _thoughtBubbleVfx?.GoAway();
        _thoughtBubbleVfx = null;
        _selectedOptionConfirmation =
            PreloadManager.Cache.GetScene(MultiplayerConfirmationScenePath).Instantiate<Control>();
        _selectedOptionConfirmation.GetNode<TextureRect>("%Icon").Texture = option.Icon;
        this.AddChildSafely(_selectedOptionConfirmation);
        var characterIndex = _characterIndex;
        var flag = characterIndex is 0 or 3;
        _selectedOptionConfirmation.GlobalPosition = GetRestSiteOptionAnchor().GlobalPosition;
        _selectedOptionConfirmation.Position += MultiplayerConfirmationOffset +
                                                (flag ? MultiplayerConfirmationFlipOffset : Vector2.Zero);
    }

    private void Shake()
    {
        TaskHelper.RunSafely(DoShake());
    }

    private async Task DoShake()
    {
        var shake = new ScreenPunchInstance(15f, 0.4, 0f);
        var originalPosition = Position;
        while (!shake.IsDone)
        {
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            var vector = shake.Update(GetProcessDeltaTime());
            Position = originalPosition + vector;
        }

        Position = originalPosition;
    }

    private Control GetRestSiteOptionAnchor()
    {
        return _characterIndex < 2 ? LeftThoughtAnchor : RightThoughtAnchor;
    }

    private async Task RemoveThoughtBubbleAfterDelay()
    {
        _thoughtBubbleGoAwayCancellation = new CancellationTokenSource();
        await Cmd.Wait(0.5f, _thoughtBubbleGoAwayCancellation.Token);
        if (!_thoughtBubbleGoAwayCancellation.IsCancellationRequested)
        {
            _thoughtBubbleVfx?.GoAway();
            _thoughtBubbleVfx = null;
        }
    }

    private IEnumerable<Node2D> GetChildSpineNodes()
    {
        return GetChildren().OfType<Node2D>().Where(item => item.GetClass() == "SpineSprite");
    }
}