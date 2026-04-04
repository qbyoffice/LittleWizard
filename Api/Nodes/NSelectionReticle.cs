using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace LittleWizard.Api.Nodes;

public partial class NSelectionReticle : Control
{
    private readonly CancellationTokenSource _cancelToken = new();
    private Tween? _currentTween;

    public bool IsSelected { get; private set; }

    public override void _Ready()
    {
        Modulate = Colors.Transparent;
        PivotOffset = Size * 0.5f;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _cancelToken.Cancel();
    }

    public void OnSelect()
    {
        if (!NCombatUi.IsDebugHideTargetingUi)
        {
            _currentTween?.Kill();
            _currentTween = CreateTween().SetParallel();
            _currentTween.TweenProperty(this, "modulate:a", 1f, 0.20000000298023224);
            _currentTween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Expo)
                .From(Vector2.One * 0.9f);
            Modulate = Colors.White;
            Scale = Vector2.One;
            IsSelected = true;
        }
    }

    public void OnDeselect()
    {
        if (!_cancelToken.IsCancellationRequested)
        {
            _currentTween?.Kill();
            if (this.IsValid() && IsInsideTree())
            {
                _currentTween = CreateTween()?.SetParallel();
                _currentTween?.TweenProperty(this, "modulate:a", 0f, 0.20000000298023224).SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Sine);
                _currentTween?.TweenProperty(this, "scale", Vector2.One * 1.05f, 0.20000000298023224)
                    .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
                IsSelected = false;
            }
        }
    }
}