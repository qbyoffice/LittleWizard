using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LittleWizard.Powers.Elements;

public class ElementTemporaryStrengthPower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Power<FireElement>();

    protected override bool IsPositive => false;
}