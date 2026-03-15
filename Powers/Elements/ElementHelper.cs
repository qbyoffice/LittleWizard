using LittleWizard.Interface;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;

namespace LittleWizard.Powers.Elements;

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

    public static void FireAndWater(Creature owner, decimal amountA, decimal amountB)
    {
        var sum = amountA + amountB;
        PowerCmd.Apply<ElementTemporaryStrengthPower>(owner, sum, null, null);
        PowerCmd.Apply<VulnerablePower>(owner, sum, null, null);
        _ = OnElementReactor(owner);
    }

    public static void FireAndEarth(Creature owner, decimal amountA, decimal amountB)
    {
        DamageCmd.Attack(amountA * amountB).Targeting(owner).Execute(null);
        _ = OnElementReactor(owner);
    }

    public static void WaterAndEarth(Creature owner, decimal amountA, decimal amountB)
    {
        if (amountA * amountB > Rng.Chaotic.NextInt(0, 100)) CreatureCmd.Stun(owner);
        _ = OnElementReactor(owner);
    }

    private static async Task OnElementReactor(Creature owner)
    {
        if (owner.CombatState == null) return;
        foreach (var player in owner.CombatState.Players)
        foreach (var power in player.Creature.Powers)
        {
            if (power is not IAfterElementReactor afterElementReactor) continue;
            await afterElementReactor.AfterElementReact(player.Creature, owner);
        }

        foreach (var power in owner.Powers)
        {
            if (power is not IAfterElementReactor afterElementReactor) continue;
            await afterElementReactor.AfterElementReact(owner, owner);
        }
    }
}