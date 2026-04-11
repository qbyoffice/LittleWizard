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
<<<<<<< Updated upstream
        if (applier != null)
            CreatureCmd.GainBlock(applier, Amount, ValueProp.Move, null);
=======
        PowerCmd.Apply<WaterEarthStrengthDecreasePower>(Owner, Amount, applier, cardSource);
        PowerCmd.Apply<ElementBlockPower>(Owner, Amount, applier, cardSource);
>>>>>>> Stashed changes
        PowerCmd.Remove(this);
        return Task.CompletedTask;
    }
}
