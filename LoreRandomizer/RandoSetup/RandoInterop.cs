using ItemChanger;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Enums;
using LoreCore.Locations;
using LoreCore.Other;
using LoreRandomizer.Menu;
using LoreRandomizer.ModInterop;
using Modding;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.Logging;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;
using RandomizerMod.Settings;
using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static LoreCore.Data.ItemList;
using static LoreCore.Data.LocationList;

namespace LoreRandomizer.RandoSetup;

internal class RandoInterop
{
    #region Event handler

    private static void WriteLoreRandoSettings(LogArguments arg1, TextWriter textWriter)
    {
        textWriter.WriteLine("Lore Randomizer settings");
        using Newtonsoft.Json.JsonTextWriter jsonTextWriter = new(textWriter) { CloseOutput = false, };
        JsonUtil._js.Serialize(jsonTextWriter, LoreRandomizer.RandoSettings);
        textWriter.WriteLine();
    }

    private static void SetupTraveller(LogicManager logicManager, GenerationSettings generationSettings, ProgressionInitializer progressionInitializer)
    {
        if (LoreRandomizer.RandoSettings.Enabled && LoreRandomizer.RandoSettings.TravellerOrder == Menu.TravellerBehaviour.None)
            foreach (Traveller traveller in Enum.GetValues(typeof(Traveller)))
                progressionInitializer.Increments.Add(new(logicManager.GetTerm(traveller.ToString().ToUpper()), 10));
    }

    private static int RandoController_OnCalculateHash(RandoController arg1, int arg2)
    {
        if (!LoreRandomizer.Instance.Settings.Enabled)
            return 0;
        int toAdd = 28;
        if (LoreRandomizer.Instance.Settings.UseCustomLore)
            toAdd += 69;
        if (LoreRandomizer.Instance.Settings.RandomizeTravellerDialogues && LoreRandomizer.Instance.Settings.TravellerOrder == Menu.TravellerBehaviour.None)
            toAdd += 420;
        return toAdd;
    }

    private static void AddLoreRando(RequestBuilder requestBuilder)
    {
        ShrineLocation.SelectedTablets.Clear();
        LoreRandomizer.Instance.GenerateLoreTablets = false;
        if (!LoreRandomizer.RandoSettings.Enabled)
            return;
        if (LoreRandomizer.RandoSettings.RandomizeNpc)
            requestBuilder.AddList(NpcItems, NpcLocations);
        if (LoreRandomizer.RandoSettings.RandomizeDreamNailDialogue)
            requestBuilder.AddList(DreamItems, DreamLocations);
        if (LoreRandomizer.RandoSettings.RandomizePointOfInterest)
            requestBuilder.AddList(PointOfInterestItems, PointOfInterestLocations);
        if (LoreRandomizer.RandoSettings.RandomizeTravellerDialogues)
            requestBuilder.AddList(TravellerItems, TravellerLocations);
        if (LoreRandomizer.RandoSettings.RandomizeElderbugRewards)
        {
            System.Random random = new(requestBuilder.gs.Seed);
            requestBuilder.AddLocationByName(Elderbug_Shop);
            requestBuilder.EditLocationRequest(Elderbug_Shop, info =>
            {
                info.getLocationDef = () => new()
                {
                    FlexibleCount = true,
                    Name = Elderbug_Shop,
                    AdditionalProgressionPenalty = requestBuilder.gs.ProgressionDepthSettings.MultiLocationPenalty
                };
                info.onRandoLocationCreation += (factory, location) =>
                {
                    Term term = requestBuilder.lm.GetTerm("LORE");
                    int maxElderbugCost = 0;
                    if (LoreRandomizer.RandoSettings.RandomizeDreamNailDialogue)
                        maxElderbugCost += DreamItems.Length;
                    if (LoreRandomizer.RandoSettings.RandomizeTravellerDialogues)
                        maxElderbugCost += TravellerItems.Length;
                    if (LoreRandomizer.RandoSettings.RandomizeNpc)
                        maxElderbugCost += NpcItems.Length;
                    if (LoreRandomizer.RandoSettings.RandomizePointOfInterest)
                        maxElderbugCost += PointOfInterestItems.Length;
                    if (LoreRandomizer.RandoSettings.UseCustomLore || requestBuilder.gs.PoolSettings.LoreTablets)
                        maxElderbugCost += LoreTablets.Length;
                    location.AddCost(new SimpleCost(term, random.Next(1, maxElderbugCost)));
                };
            });
            requestBuilder.CostConverters.Subscribe(500f, GetCost);
        }
        if (LoreRandomizer.RandoSettings.RandomizeShrineOfBelievers)
        {
            System.Random random = new(requestBuilder.gs.Seed);
            List<string> viableTablets = ShrineLocation.ShrineLocations.ToList();
            // Certain shrines don't appear in some settings.
            if (requestBuilder.gs.MiscSettings.SteelSoul)
                viableTablets.Remove(ShadeKillShrine);
            if (requestBuilder.gs.CursedSettings.MaximumGrubsReplacedByMimics > 0)
            {
                viableTablets.Remove(AllGrubsShrine);
                if (requestBuilder.gs.CursedSettings.MaximumGrubsReplacedByMimics >= 23)
                    viableTablets.Remove(HalfGrubsShrine);
            }

            for (int i = 0; i < random.Next(1, viableTablets.Count); i++)
            {
                string pickedLocation = viableTablets[random.Next(0, viableTablets.Count)];
                viableTablets.Remove(pickedLocation);
                requestBuilder.AddLocationByName(pickedLocation);
                ShrineLocation.SelectedTablets.Add(pickedLocation);
            }
        }
        if (LoreRandomizer.RandoSettings.CursedListening)
            requestBuilder.AddItemByName(Listen_Ability);
        if (LoreRandomizer.RandoSettings.CursedReading)
            requestBuilder.AddItemByName(Read_Ability);
        if (LoreRandomizer.RandoSettings.UseCustomLore)
        {
            if (requestBuilder.gs.PoolSettings.LoreTablets)
                foreach (string loreTablet in LoreTablets)
                {
                    requestBuilder.RemoveItemByName(loreTablet);
                    requestBuilder.AddItemByName(loreTablet + "_Empowered");
                }
            else
            {
                LoreRandomizer.Instance.GenerateLoreTablets = true;
                foreach (string loreTablet in LoreTablets)
                    if (loreTablet != ItemNames.Lore_Tablet_Kings_Pass_Focus && loreTablet != ItemNames.Lore_Tablet_World_Sense)
                        requestBuilder.AddToVanilla(loreTablet + "_Empowered", loreTablet);

                // Focus and world sense block the lore tablet if it isn't randomized as well. Because of that, we just give the items add the start.
                if (requestBuilder.gs.NoveltySettings.RandomizeFocus)
                    requestBuilder.AddToStart(ItemNames.Lore_Tablet_Kings_Pass_Focus + "_Empowered");
                else
                    requestBuilder.AddToVanilla(ItemNames.Lore_Tablet_Kings_Pass_Focus + "_Empowered", LocationNames.Lore_Tablet_Kings_Pass_Focus);

                if (requestBuilder.gs.PoolSettings.Dreamers)
                    requestBuilder.AddToStart(ItemNames.Lore_Tablet_World_Sense + "_Empowered");
                else
                    requestBuilder.AddToVanilla(ItemNames.Lore_Tablet_World_Sense + "_Empowered", LocationNames.Lore_Tablet_World_Sense);
            }
        }
    }

    private static void ModifyLogic(GenerationSettings settings, LogicManagerBuilder builder)
    {
        if (!LoreRandomizer.RandoSettings.Enabled)
            return;
        if (LoreRandomizer.RandoSettings.CursedListening)
        {
            Term listenTerm = builder.GetOrAddTerm("LISTEN");
            builder.AddItem(new BoolItem(Listen_Ability, listenTerm));
            using Stream listenStream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.ListenLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.LogicEdit, listenStream);
        }
        if (LoreRandomizer.RandoSettings.CursedReading)
        {
            Term readTerm = builder.GetOrAddTerm("READ");
            builder.AddItem(new BoolItem(Read_Ability, readTerm));
            using Stream readStream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.ReadLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.LogicEdit, readStream);
        }
        using Stream waypointStream = ResourceHelper.LoadResource<LoreRandomizer>("Waypoints.json");
        builder.DeserializeJson(LogicManagerBuilder.JsonType.Waypoints, waypointStream);
        Term loreTerm = builder.GetOrAddTerm("LORE");
        if (LoreRandomizer.RandoSettings.RandomizeElderbugRewards)
            builder.AddLogicDef(new("Elderbug_Shop", "Town"));
        if (LoreRandomizer.RandoSettings.RandomizeNpc)
        {
            using Stream stream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.NpcLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, stream);
            foreach (string item in NpcItems)
                if (item == Dialogue_Bretta)
                {
                    Term bretta = builder.GetOrAddTerm("BRETTA");
                    builder.AddItem(new MultiItem(item, new TermValue[]
                        {
                            new(loreTerm, 1),
                            new(bretta, 1)
                        }));
                    // Lock Bretta's house behind her dialogue item, rather than the location.
                    builder.DoLogicEdit(new("Rescued_Bretta", "BRETTA"));
                }
                else
                    builder.AddItem(new SingleItem(item, new(loreTerm, 1)));
        }
        if (LoreRandomizer.RandoSettings.RandomizeDreamNailDialogue)
        {
            using Stream stream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.DreamLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, stream);
            
            foreach (string item in DreamItems)
                if (item == Grimm_Summoner_Dream)
                {
                    Term summoner = builder.GetOrAddTerm("Grimm_Summoner_Dream");
                    builder.AddItem(new MultiItem(item, new TermValue[]
                        {
                            new(loreTerm, 1),
                            new(summoner, 1)
                        }));
                    // Lock grimm behind the grimm summoner dream dialogue, rather than the location.
                    builder.DoLogicEdit(new("Nightmare_Lantern_Lit", "Grimmchild | Grimm_Summoner_Dream"));
                }
                else
                    builder.AddItem(new SingleItem(item, new(loreTerm, 1)));
            // The pale King items cannot be obtained until their dream dialogue has been acquired.
            builder.DoLogicEdit(new(LocationNames.King_Fragment, "(ORIG) + DREAMNAIL"));
        }
        if (LoreRandomizer.RandoSettings.RandomizePointOfInterest)
        {
            using Stream stream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.PointOfInterestLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, stream);
            foreach (string item in PointOfInterestItems)
                builder.AddItem(new SingleItem(item, new(loreTerm, 1)));
        }
        if (LoreRandomizer.RandoSettings.RandomizeTravellerDialogues)
        {
            foreach (Traveller traveller in Enum.GetValues(typeof(Traveller)))
                builder.GetOrAddTerm(traveller.ToString().ToUpper());
            using Stream stream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.TravellerLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, stream);
            foreach (string item in TravellerItems)
            {
                string traveller = "QUIRREL";
                if (item.Contains("Zote"))
                    traveller = "ZOTE";
                else if (item.Contains("Tiso"))
                    traveller = "TISO";
                else if (item.Contains("Cloth"))
                    traveller = "CLOTH";
                else if (item.Contains("Hornet"))
                    traveller = "HORNET";

                builder.AddItem(new MultiItem(item, new TermValue[]
                {
                    new(loreTerm, 1),
                    new(builder.GetOrAddTerm(traveller), 1)
                }));
            }
            foreach (Traveller traveller in TravellerLocation.Stages.Keys.ToList())
                TravellerLocation.Stages[traveller] = LoreRandomizer.RandoSettings.TravellerOrder == Menu.TravellerBehaviour.None
                    ? 10
                    : 0;
            // Since traveller (including Hornet) only appear once a certain stage is reached, we need to modify the second hornet waypoint as well.
            if (LoreRandomizer.RandoSettings.TravellerOrder == Menu.TravellerBehaviour.Vanilla)
            { 
                builder.DoLogicEdit(new("Defeated_Hornet_2", "(ORIG) + HORNET>1"));
                builder.DoLogicEdit(new("Defeated_Uumuu", "(ORIG) + QUIRREL>7"));
                builder.DoLogicEdit(new("Rescued_Deepnest_Zote", "ZOTE>4"));
            }
        }
        if (LoreRandomizer.RandoSettings.RandomizeShrineOfBelievers)
        {
            using Stream stream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.ShrineLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, stream);

            // If grub tolerance is used, the internal counter is set to a negative value, making it impossible to reach 46.
            // Because of this, we set the max and half amount via the tolerance instead.
            int logicallyRequiredGrubs = 46 - settings.CostSettings.GrubTolerance;
            builder.DoLogicEdit(new("All_Grubs-Shrine", "(ORIG) + GRUBS>" + (logicallyRequiredGrubs - 1)));
            builder.DoLogicEdit(new("Half_Grubs-Shrine", "(ORIG) + GRUBS>" + (logicallyRequiredGrubs / 2 - 1)));
        }
        if (LoreRandomizer.RandoSettings.UseCustomLore)
            foreach (string item in LoreTablets.Select(x => x + "_Empowered"))
                builder.AddItem(new SingleItem(item, new(loreTerm, 1)));
        else if (settings.PoolSettings.LoreTablets)
            foreach (string item in LoreTablets)
                builder.AddItem(new SingleItem(item, new(loreTerm, 1)));
    }

    private static bool GetCost(LogicCost logicCost, out Cost cost)
    {
        if (logicCost.GetTerms()?.Any(x => x.Name == "LORE") == true && logicCost is SimpleCost simpleCost)
        {
            cost = new LoreCost()
            {
                NeededLore = simpleCost.threshold
            };
            return true;
        }
        else
            cost = default;
        return false;
    }

    #endregion

    #region Methods

    public static void Initialize()
    {
        RandoController.OnCalculateHash += RandoController_OnCalculateHash;
        RequestBuilder.OnUpdate.Subscribe(30f, AddLoreRando);
        RCData.RuntimeLogicOverride.Subscribe(9999f, ModifyLogic);
        ProgressionInitializer.OnCreateProgressionInitializer += SetupTraveller;
        SettingsLog.AfterLogSettings += WriteLoreRandoSettings;

        if (ModHooks.GetMod("RandoSettingsManager") is Mod)
            HookRandoSettingsManager();

        CondensedSpoilerLogger.AddCategory("Special Items", () => LoreRandomizer.RandoSettings.Enabled 
        && (LoreRandomizer.RandoSettings.CursedListening | LoreRandomizer.RandoSettings.CursedReading), new()
        {
            Listen_Ability,
            Read_Ability
        });
        CondensedSpoilerLogger.AddCategory("Important traveller level", () => LoreRandomizer.RandoSettings.Enabled && LoreRandomizer.RandoSettings.RandomizeTravellerDialogues 
        && LoreRandomizer.RandoSettings.TravellerOrder == TravellerBehaviour.Vanilla, new()
        {
            Dialogue_Quirrel_Crossroads,
            Dialogue_Quirrel_Greenpath,
            Dialogue_Quirrel_Queen_Station,
            Dialogue_Quirrel_Mantis_Village,
            Dialogue_Quirrel_City,
            Dialogue_Quirrel_Peaks,
            Dialogue_Quirrel_Deepnest,
            Dialogue_Quirrel_Outside_Archive,
            Dialogue_Quirrel_Archive,
            Dialogue_Quirrel_Blue_Lake,
            Dialogue_Zote_Greenpath,
            Dialogue_Zote_Dirtmouth_Intro,
            Dialogue_Zote_City,
            Dialogue_Zote_Deepnest,
            Dialogue_Zote_Colosseum,
            Dialogue_Zote_Dirtmouth_After_Colosseum,
            Dialogue_Hornet_Greenpath,
            Dialogue_Hornet_Fountain,
            Dialogue_Hornet_Edge,
            Dialogue_Hornet_Abyss,
            Dialogue_Hornet_Deepnest,
            Dialogue_Hornet_Temple
        });
    }

    private static void HookRandoSettingsManager()
    {
        RandoSettingsManagerMod.Instance.RegisterConnection(new SimpleSettingsProxy<RandoSettings>(LoreRandomizer.Instance,
        ConnectionMenu.Instance.PassRSMSettings,
        () => LoreRandomizer.RandoSettings.Enabled ? LoreRandomizer.RandoSettings : null));
    }

    #endregion
}
