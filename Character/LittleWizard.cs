using BaseLib.Abstracts;
using Godot;
using LittleWizard.Cards.Basic;
using LittleWizard.Enchantments;
using LittleWizard.Relics;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;

namespace LittleWizard.Character;

public class LittleWizard : PlaceholderCharacterModel
{
    public static readonly Color CharacterColor = new("384A61");
    public const string InnerName = "little_wizard";

    public override Color NameColor => CharacterColor;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 76;

    public override CardPoolModel CardPool => ModelDb.CardPool<LittleWizardCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<LittleWizardRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<LittleWizardPotionPool>();

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeLittleWizard>(),
        Utils.EnchantedCard(ModelDb.Card<StrikeLittleWizard>(), ModelDb.Enchantment<FireEnchantment>()),
        Utils.EnchantedCard(ModelDb.Card<StrikeLittleWizard>(), ModelDb.Enchantment<WaterEnchantment>()),
        Utils.EnchantedCard(ModelDb.Card<StrikeLittleWizard>(), ModelDb.Enchantment<EarthEnchantment>()),
        ModelDb.Card<ColorfulBalls>(),
        ModelDb.Card<DefendLittleWizard>(),
        ModelDb.Card<DefendLittleWizard>(),
        ModelDb.Card<DefendLittleWizard>(),
        ModelDb.Card<DefendLittleWizard>(),
        ModelDb.Card<Callback>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics => [
        ModelDb.Relic<FireElementGem>()
    ];
}