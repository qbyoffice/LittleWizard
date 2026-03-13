using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;

namespace LittleWizard.Powers;

#nullable enable
public static class ElementHelper
{
    public static async Task RandomElement(Creature target,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        var randomElement = Rng.Chaotic.NextInt(0, 3);
        switch (randomElement)
        {
            case 0:
            {
                await PowerCmd.Apply<FireElement>(target, amount, applier, cardSource);
                return;
            }
            case 1:
            {
                await PowerCmd.Apply<WaterElement>(target, amount, applier, cardSource);
                return;
            }
            case 2:
            {
                await PowerCmd.Apply<EarthElement>(target, amount, applier, cardSource);
                return;
            }
        }
    }

    public static void FireAndWater(FireElement fireElement, WaterElement waterElement)
    {
        PowerCmd.Apply<StrengthPower>(fireElement.Owner, -1 * (fireElement.Amount + waterElement.Amount), null, null);
        PowerCmd.Apply<VulnerablePower>(fireElement.Owner, fireElement.Amount + waterElement.Amount, null, null);
    }

    public static void FireAndEarth(FireElement fireElement, EarthElement earthElement)
    {
        DamageCmd.Attack(fireElement.Amount * earthElement.Amount).Targeting(fireElement.Owner).Execute(null);
    }

    public static void WaterAndEarth(WaterElement waterElement, EarthElement earthElement)
    {
        if (waterElement.Amount * earthElement.Amount > Rng.Chaotic.NextInt(0, 100))
        {
            CreatureCmd.Stun(waterElement.Owner);
        }
    }
}