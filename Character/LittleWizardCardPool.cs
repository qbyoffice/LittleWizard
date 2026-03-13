using BaseLib.Abstracts;
using Godot;

namespace LittleWizard.Character;

public partial class LittleWizardCardPool : CustomCardPoolModel
{
    public override string Title => LittleWizard.InnerName;

    public override string EnergyColorName => LittleWizard.InnerName;

    // public override Color ShaderColor => new("FFFFFF");

    public override Color DeckEntryCardColor => LittleWizard.CharacterColor;

    // public override Color EnergyOutlineColor => new("000000");

    public override bool IsColorless => false;
}