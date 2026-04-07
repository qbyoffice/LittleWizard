using LittleWizard.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LittleWizard.Powers.Elements.Reacts;

public class WaterAndEarthElementReactorPower : LittleWizardPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (applier != null)
            CreatureCmd.GainBlock(applier, Amount, ValueProp.Move, null);
        PowerCmd.Remove(this);
        return Task.CompletedTask;
    }
}
