using LittleWizard.Api.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LittleWizard.Powers.Elements.Reacts;

public class FireAndWaterElementReactorPower : LittleWizardPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
<<<<<<< Updated upstream
        PowerCmd.Apply<ElementTemporaryStrengthPower>(Owner, Amount, applier, cardSource);
        PowerCmd.Apply<VulnerablePower>(Owner, Amount, applier, cardSource);
=======
        PowerCmd.Apply<FireWaterStrengthDecreasePower>(Owner, Amount, applier, cardSource);
        CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),
            Owner,
            Amount,
            ValueProp.Unpowered,
            applier,
            cardSource
        );
>>>>>>> Stashed changes
        PowerCmd.Remove(this);
        return Task.CompletedTask;
    }
}
