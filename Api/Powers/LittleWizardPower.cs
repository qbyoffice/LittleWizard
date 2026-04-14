using BaseLib.Abstracts;
using LittleWizard.Api.Extensions;

namespace LittleWizard.Api.Powers;

public abstract class LittleWizardPower : CustomPowerModel
{
    private string GetIconBaseName() => Id.Entry.RemovePrefix().ToLowerInvariant();

    public override string CustomPackedIconPath =>
        $"res://{MainFile.ModId}/images/powers/{GetIconBaseName()}.png";

    public override string CustomBigIconPath =>
        $"res://{MainFile.ModId}/images/powers/{GetIconBaseName()}.png";
}
