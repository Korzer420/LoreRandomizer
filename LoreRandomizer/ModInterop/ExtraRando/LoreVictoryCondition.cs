using ExtraRando.Data;
using LoreCore.Items;
using RandomizerCore.Logic;
using System;
using static LoreCore.Data.ItemList;

namespace LoreRandomizer.ModInterop.ExtraRando;

public class LoreVictoryCondition : IVictoryCondition
{
    public int CurrentAmount { get; set; }

    public int RequiredAmount { get; set; }

    public int ClampAvailableRange(int setAmount)
    {
        if (!LoreRandomizer.RandoSettings.Enabled)
            return 0;
        int maximumLore = 0;
        if (LoreRandomizer.RandoSettings.RandomizeNpc)
            maximumLore += NpcItems.Length;
        if (LoreRandomizer.RandoSettings.RandomizeDreamNailDialogue)
            maximumLore += DreamItems.Length;
        if (LoreRandomizer.RandoSettings.RandomizePointOfInterest)
            maximumLore += PointOfInterestItems.Length;
        if (LoreRandomizer.RandoSettings.RandomizeTravellerDialogues)
            maximumLore += TravellerItems.Length;
        if (LoreRandomizer.RandoSettings.AddZotePrecepts)
            maximumLore += 57;
        if (LoreRandomizer.RandoSettings.UseCustomLore)
            maximumLore += LoreTablets.Length;
        return Math.Max(Math.Min(maximumLore, setAmount), 0);
    }

    public string GetHintText() => this.GenerateHintText("Knowledge is hidden at:", x => x is PowerLoreItem);

    public string GetMenuName() => "Lore";

    public string PrepareLogic(LogicManagerBuilder logicBuilder)
    {
        logicBuilder.GetOrAddTerm("LORE", TermType.Int);
        return $"LORE>{RequiredAmount - 1}";
    }

    public void StartListening() => PowerLoreItem.AcquirePowerItem += PowerLoreItem_AcquirePowerItem;

    public void StopListening() => PowerLoreItem.AcquirePowerItem -= PowerLoreItem_AcquirePowerItem;

    private string PowerLoreItem_AcquirePowerItem(string key, string originalText)
    {
        CurrentAmount++;
        this.CheckForEnding();
        return originalText;
    }
}
