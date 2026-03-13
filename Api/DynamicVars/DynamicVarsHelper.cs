using LittleWizard.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LittleWizard.Localization.DynamicVars;

public static class DynamicVarsHelper
{
    public static RandomElementVar GetRandomElementVar(DynamicVarSet varSet)
    {
        return (RandomElementVar) varSet[RandomElementVar.DefaultName];
    }

    public static PowerVar<T> GetPowerVar<T>(DynamicVarSet varSet) where T : PowerModel
    {
        return (PowerVar<T>) varSet[typeof(T).Name];
    }
}