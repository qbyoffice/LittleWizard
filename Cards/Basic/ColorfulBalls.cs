using BaseLib.Utils;
using LittleWizard.Cards.Interface;
using LittleWizard.Localization.DynamicVars;
using LittleWizard.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LittleWizard.Cards.Basic;

public sealed class ColorfulBalls() : LittleWizardCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy), IElementCard
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6, ValueProp.Move),
        new RandomElementVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        for (int i = 0; i < DynamicVars[RandomElementVar.DefaultName].BaseValue; i++)
        {
            await ElementHelper.RandomElement(play.Target, 1, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
        DynamicVars[RandomElementVar.DefaultName].UpgradeValueBy(1m);
    }
}