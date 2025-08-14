using ExtraRando.Data;
using LoreCore.Items;
using RandomizerCore.Logic;
using System;

namespace LoreRandomizer.ModInterop.ExtraRando;

public class PreceptVictoryCondition : IVictoryCondition
{
    public int CurrentAmount { get; set; }

    public int RequiredAmount { get; set; }

    public int ClampAvailableRange(int setAmount) => LoreRandomizer.RandoSettings.AddZotePrecepts && LoreRandomizer.RandoSettings.Enabled
        ? Math.Min(57, Math.Max(0, setAmount))
        : 0;

    public string GetHintText() => this.GenerateHintText("Your hear yappin from:", x => x is PowerLoreItem item && item.name.StartsWith("Precept"));

    public string GetMenuName() => "Zote Precepts";

    public string PrepareLogic(LogicManagerBuilder logicBuilder)
    {
        logicBuilder.GetOrAddTerm("PRECEPTS", TermType.Int);
        return $"PRECEPTS>{RequiredAmount - 1}";
    }

    public void StartListening() => PowerLoreItem.AcquirePowerItem += PowerLoreItem_AcquirePowerItem;

    public void StopListening() => PowerLoreItem.AcquirePowerItem -= PowerLoreItem_AcquirePowerItem;

    private string PowerLoreItem_AcquirePowerItem(string key, string originalText)
    {
        if (key.StartsWith("PRECEPT_"))
        {
            CurrentAmount++;
            this.CheckForEnding();
        }
        return originalText;
    }
}
