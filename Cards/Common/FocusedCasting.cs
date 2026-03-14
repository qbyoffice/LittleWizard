using System.Diagnostics;
using LittleWizard.Api.DynamicVars;
using LittleWizard.Cards.Interface;
using LittleWizard.Powers.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LittleWizard.Cards.Common;

public class FocusedCasting() : LittleWizardCard(1, CardType.Skill, CardRarity.Common, TargetType.Self), IElementCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FocusedCastingPower>(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<FocusedCastingPower>(cardPlay.Target, DynamicVarsHelper.GetPowerVar<FocusedCastingPower>(DynamicVars).BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVarsHelper.GetPowerVar<FocusedCastingPower>(DynamicVars).UpgradeValueBy(1);
    }
}