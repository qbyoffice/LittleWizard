using BaseLib.Abstracts;
using BaseLib.Utils;
using LittleWizard.Character;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace LittleWizard.Cards;

[Pool(typeof(LittleWizardCardPool))]
public abstract class LittleWizardCard(
    int baseCost,
    CardType type,
    CardRarity rarity,
    TargetType target,
    bool showInCardLibrary = true,
    bool autoAdd = true)
    : CustomCardModel(baseCost, type, rarity, target, showInCardLibrary, autoAdd)
{
}