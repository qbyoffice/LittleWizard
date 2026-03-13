using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LittleWizard.Powers;

public class FireElement : LittleWizardPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side)
        {
            return;
        }

        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),
            Owner,
            Amount,
            ValueProp.Unblockable | ValueProp.Unpowered,
            (Creature)null,
            (CardModel)null
            );
        if (!Owner.IsAlive)
        {
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
    }

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature applier,
        out decimal modifiedAmount)
    {
        if (target != Owner)
        {
            modifiedAmount = amount;
            return false;
        }

        switch (canonicalPower)
        {
            case WaterElement water:
            {
                ElementHelper.FireAndWater(this, water);
                modifiedAmount = 0;
                return true;
            }
            case EarthElement earth:
            {   
                ElementHelper.FireAndEarth(this, earth);
                modifiedAmount = 0;
                return true;
            }
            default:
            {
                modifiedAmount = amount;
                return false;    
            }
        }
    }

    public override async Task AfterModifyingPowerAmountReceived(PowerModel power)
    {
        await PowerCmd.Remove(this);
    }
}