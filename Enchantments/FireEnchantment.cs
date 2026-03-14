using LittleWizard.Cards.Interface;
using LittleWizard.Powers;
using LittleWizard.Powers.Elements;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LittleWizard.Enchantments;

public sealed class FireEnchantment : EnchantmentModel, IElementCard
{
    public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;

    public override bool ShowAmount => true;

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay is { Target: not null })
        {
            await PowerCmd.Apply<FireElement>(cardPlay.Target, 1, cardPlay.Card.Owner.Creature, cardPlay.Card);
        }
    }
}