using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;

namespace LittleWizard.Api.Nodes;

public partial class NCardTrailVfx : Node2D
{
    public required Control NodeToFollow;

    public required Node2D Sprites;

    private Tween? _tween;

    private bool _updateSprites = true;

    public static NCardTrailVfx? Create(Control card, string characterTrailPath)
    {
        if (TestMode.IsOn) return null;
        var nCardTrailVfx = PreloadManager.Cache.GetScene(characterTrailPath).Instantiate<NCardTrailVfx>();
        nCardTrailVfx.NodeToFollow = card;
        return nCardTrailVfx;
    }

    public override void _Ready()
    {
        if (NCombatUi.IsDebugHidingPlayContainer) Visible = false;
        Sprites = GetNode<Node2D>("Sprites");
        Sprites.Modulate = StsColors.transparentWhite;
        _tween = CreateTween().SetParallel();
        _tween.TweenProperty(Sprites, "scale", Vector2.One * 0.5f, 0.5).SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetDelay(0.25);
        _tween.TweenProperty(NodeToFollow, "modulate:a", 0.75f, 0.5).SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenProperty(Sprites, "modulate:a", 1f, 1.0).SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
    }

    public override void _Process(double delta)
    {
        if (!_updateSprites) return;
        GlobalPosition = NodeToFollow.GlobalPosition;
        Rotation = NodeToFollow.Rotation;
    }

    public async Task FadeOut()
    {
        _updateSprites = false;
        _tween = CreateTween().SetParallel();
        _tween.TweenProperty(this, "modulate:a", 0f, 0.5);
        StopParticles(_tween);
        await ToSignal(_tween, Tween.SignalName.Finished);
        this.QueueFreeSafely();
    }

    private void StopParticles(Tween tween)
    {
        foreach (var child in Sprites.GetChildren())
            if (child is CpuParticles2D cpuParticles2D)
                tween.TweenProperty(cpuParticles2D, "amount", 1, 0.5);
    }

    public override void _ExitTree()
    {
        _tween?.Kill();
    }
}