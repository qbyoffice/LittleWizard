using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LittleWizard.Powers;

public class WaterElement : LittleWizardPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Side)
        {
            return;
        }

        await PowerCmd.Apply<WeakPower>(Owner, Amount, null, null);
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
            case FireElement fire:
            {
                ElementHelper.FireAndWater(fire, this);
                modifiedAmount = 0;
                return true;
            }
            case EarthElement earth:
            {   
                ElementHelper.WaterAndEarth(this, earth);
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