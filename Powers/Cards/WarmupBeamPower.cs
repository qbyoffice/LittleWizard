using LittleWizard.Api.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace LittleWizard.Powers.Cards;

public sealed class WarmupBeamPower : BaseMoreElementReceivedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
