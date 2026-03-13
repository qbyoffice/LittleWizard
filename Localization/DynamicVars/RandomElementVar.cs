using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LittleWizard.Localization.DynamicVars;

public class RandomElementVar(decimal baseValue) : DynamicVar(DefaultName, baseValue)
{   
    public const string DefaultName = "RandomElement";
}