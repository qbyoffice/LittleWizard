using MegaCrit.Sts2.Core.Models;

namespace LittleWizard;

public static class Utils
{
    public static CardModel EnchantedCard(CardModel cardModel, EnchantmentModel enchantmentModel)
    {
        cardModel.EnchantInternal(enchantmentModel, 1);
        return cardModel;
    }
}