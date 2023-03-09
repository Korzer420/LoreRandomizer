using ItemChanger;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Enums;
using LoreCore.Locations;
using LoreCore.Other;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.RC;
using RandomizerMod.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static LoreCore.Data.ItemList;
using static LoreCore.Data.LocationList;

namespace LoreRandomizer.RandoSetup;

internal class RandoInterop
{
    #region Members

    internal static string[] NpcLocations { get; } = new string[]
    {
        Bretta,
        Bardoon,
        Vespa,
        Midwife,
        Myla,
        Willoh,
        Marissa,
        Joni,
        Grasshopper,
        Mask_Maker,
        Emilitia,
        Fluke_Hermit,
        Moss_Prophet,
        Queen,
        Dung_Defender,
        Menderbug_Diary,
        Gravedigger,
        Poggy,
        Godseeker,
        Millibelle,
        Hidden_Moth
    };

    internal static string[] NpcItems = new string[]
    {
        Dialogue_Bretta,
        Dialogue_Bardoon,
        Dialogue_Vespa,
        Dialogue_Midwife,
        Dialogue_Myla,
        Dialogue_Willoh,
        Dialogue_Marissa,
        Dialogue_Joni,
        Dialogue_Grasshopper,
        Dialogue_Mask_Maker,
        Dialogue_Emilitia,
        Dialogue_Fluke_Hermit,
        Dialogue_Moss_Prophet,
        Dialogue_Queen,
        Dialogue_Dung_Defender,
        Dialogue_Menderbug_Diary,
        Dialogue_Gravedigger,
        Dialogue_Poggy,
        Dialogue_Godseeker,
        Dialogue_Millibelle,
        Dialogue_Hidden_Moth
    };

    internal static string[] DreamLocations = new string[]
    {
        Ancient_Nailsmith_Golem_Dream,
        Aspid_Queen_Dream,
        Crystalized_Shaman_Dream,
        Dashmaster_Statue_Dream,
        Dream_Shield_Statue_Dream,
        Dryya_Dream,
        Grimm_Summoner_Dream,
        Hopper_Dummy_Dream,
        Isma_Dream,
        Kings_Mould_Machine_Dream,
        Mine_Golem_Dream,
        Overgrown_Shaman_Dream,
        Pale_King_Dream,
        Radiance_Statue_Dream,
        Shade_Golem_Dream_Normal,
        Shade_Golem_Dream_Void,
        Shriek_Statue_Dream,
        Shroom_King_Dream,
        Snail_Shaman_Tomb_Dream
    };

    internal static string[] DreamItems = new string[]
    {
        Dream_Dialogue_Ancient_Nailsmith_Golem,
        Dream_Dialogue_Aspid_Queen,
        Dream_Dialogue_Crystalized_Shaman,
        Dream_Dialogue_Dashmaster_Statue,
        Dream_Dialogue_Dream_Shield_Statue,
        Dream_Dialogue_Dryya,
        Dream_Dialogue_Grimm_Summoner,
        Dream_Dialogue_Hopper_Dummy,
        Dream_Dialogue_Isma,
        Dream_Dialogue_Kings_Mould_Machine,
        Dream_Dialogue_Mine_Golem,
        Dream_Dialogue_Overgrown_Shaman,
        Dream_Dialogue_Pale_King,
        Dream_Dialogue_Radiance_Statue,
        Dream_Dialogue_Shade_Golem_Normal,
        Dream_Dialogue_Shade_Golem_Void,
        Dream_Dialogue_Shriek_Statue,
        Dream_Dialogue_Shroom_King,
        Dream_Dialogue_Snail_Shaman_Tomb
    };

    internal static string[] PointOfInterestLocations = new string[]
    {
        City_Fountain,
        Dreamer_Tablet,
        Weaver_Seal,
        Grimm_Machine,
        Beast_Den_Altar,
        Garden_Golem,
        Grub_Seal,
        White_Palace_Nursery,
        Grimm_Summoner_Corpse,
        Stag_Nest,
        LocationList.Lore_Tablet_Record_Bela,
        LocationList.Traitor_Grave,
        Elder_Hu_Grave,
        Gorb_Grave,
        Marmu_Grave,
        Xero_Grave,
        No_Eyes_Statue,
        Markoth_Corpse,
        Galien_Corpse
    };

    internal static string[] PointOfInterestItems = new string[]
    {
        Inscription_City_Fountain,
        Inscription_Dreamer_Tablet,
        Inspect_Weaver_Seal,
        Inspect_Grimm_Machine,
        Inspect_Beast_Den_Altar,
        Inspect_Garden_Golem,
        Inspect_Grub_Seal,
        Inspect_White_Palace_Nursery,
        Inspect_Grimm_Summoner_Corpse,
        ItemList.Lore_Tablet_Record_Bela,
        ItemList.Traitor_Grave,
        Inspect_Elder_Hu,
        Inspect_Gorb,
        Inspect_Marmu,
        Inspect_Xero,
        Inspect_No_Eyes,
        Inspect_Markoth,
        Inspect_Galien
    };

    internal static string[] TravellerLocations = new string[]
    {
        Quirrel_Crossroads,
        Quirrel_Greenpath,
        Quirrel_Queen_Station,
        Quirrel_Mantis_Village,
        Quirrel_City,
        Quirrel_Deepnest,
        Quirrel_Peaks,
        Quirrel_Outside_Archive,
        Quirrel_After_Monomon,
        Quirrel_Blue_Lake,
        Cloth_Fungal_Wastes,
        Cloth_Basin,
        Cloth_Deepnest,
        Cloth_Garden,
        Cloth_End,
        Tiso_Dirtmouth,
        Tiso_Crossroads,
        Tiso_Blue_Lake,
        Tiso_Colosseum,
        Tiso_Corpse,
        Zote_Greenpath,
        Zote_Dirtmouth_Intro,
        Zote_City,
        Zote_Deepnest,
        Zote_Colosseum,
        Zote_Dirtmouth_After_Colosseum,
        Hornet_Greenpath,
        Hornet_Fountain,
        Hornet_Edge,
        Hornet_Abyss,
        Hornet_Deepnest,
        Hornet_Temple
    };

    internal static string[] TravellerItems = new string[]
    {
        Dialogue_Quirrel_Crossroads,
        Dialogue_Quirrel_Greenpath,
        Dialogue_Quirrel_Queen_Station,
        Dialogue_Quirrel_Mantis_Village,
        Dialogue_Quirrel_City,
        Dialogue_Quirrel_Deepnest,
        Dialogue_Quirrel_Peaks,
        Dialogue_Quirrel_Outside_Archive,
        Dialogue_Quirrel_Archive,
        Dialogue_Quirrel_Blue_Lake,
        Dialogue_Cloth_Fungal_Wastes,
        Dialogue_Cloth_Basin,
        Dialogue_Cloth_Deepnest,
        Dialogue_Cloth_Garden,
        Dialogue_Cloth_Ghost,
        Dialogue_Tiso_Dirtmouth,
        Dialogue_Tiso_Crossroads,
        Dialogue_Tiso_Blue_Lake,
        Dialogue_Tiso_Colosseum,
        Dream_Dialogue_Tiso_Corpse,
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
    };

    private static List<string> _selectedTablets = new();

    #endregion

    #region Methods

    public static void Initialize()
    {
        RandoController.OnCalculateHash += RandoController_OnCalculateHash;
        RequestBuilder.OnUpdate.Subscribe(30f, AddLoreRando);
        RCData.RuntimeLogicOverride.Subscribe(9999f, ModifyLogic);
        ProgressionInitializer.OnCreateProgressionInitializer += SetupTraveller;
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
        _selectedTablets.Clear();
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
                        location.AddCost(new SimpleCost(term, random.Next(1, 60)));
                    };
            });
            requestBuilder.CostConverters.Subscribe(500f, GetCost);
        }
        if (LoreRandomizer.RandoSettings.RandomizeShrineOfBelievers)
        {
            System.Random random = new(requestBuilder.gs.Seed);
            List<string> viableTablets = ShrineLocation.ShrineLocations.ToList();

            for (int i = 0; i < 20; i++)
            {
                string pickedLocation = viableTablets[random.Next(0, viableTablets.Count)];
                viableTablets.Remove(pickedLocation);
                requestBuilder.AddLocationByName(pickedLocation);
                _selectedTablets.Add(pickedLocation);
            }
        }
    }

    private static void ModifyLogic(GenerationSettings settings, LogicManagerBuilder builder)
    {
        if (!LoreRandomizer.RandoSettings.Enabled)
            return;
        using Stream waypointStream = ResourceHelper.LoadResource<LoreRandomizer>("Waypoints.json");
        builder.DeserializeJson(LogicManagerBuilder.JsonType.Waypoints, waypointStream);
        Term loreTerm = builder.GetOrAddTerm("LORE");
        if (LoreRandomizer.RandoSettings.RandomizeElderbugRewards)
            builder.AddLogicDef(new("Elderbug_Shop", "Town"));
        if (LoreRandomizer.RandoSettings.RandomizeNpc)
        {
            using Stream stream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.NpcLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, stream);
            // Lock Bretta's house behind her dialogue item, rather than the location.
            builder.DoSubst(new("Rescued_Bretta", "Fungus2_23 + (FULLCLAW + FULLDASH | FULLCLAW + FULLSUPERDASH | LEFTCLAW + WINGS | RIGHTCLAW + ENEMYPOGOS + WINGS | COMPLEXSKIPS + FULLCLAW + $SHADESKIP[2HITS] + SPELLAIRSTALL + $CASTSPELL[1,1,before:ROOMSOUL] + $TAKEDAMAGE[2])",
                "BRETTA"));
            foreach (string item in NpcItems)
                if (item == Dialogue_Bretta)
                {
                    Term bretta = builder.GetOrAddTerm("BRETTA");
                    builder.AddItem(new MultiItem(item, new TermValue[]
                        {
                            new(loreTerm, 1),
                            new(bretta, 1)
                        }));
                }
                else
                    builder.AddItem(new SingleItem(item, new(loreTerm, 1)));
        }
        if (LoreRandomizer.RandoSettings.RandomizeDreamNailDialogue)
        {
            using Stream stream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.DreamLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, stream);
            // Lock grimm behind the grimm summoner dream dialogue, rather than the location.
            builder.DoSubst(new("Rescued_Bretta", "Cliffs_06[left1] + DREAMNAIL | Grimmchild",
                "Grimm_Summoner_Dream | Grimmchild"));
            foreach (string item in DreamItems)
                if (item == Grimm_Summoner_Dream)
                {
                    Term summoner = builder.GetOrAddTerm("Grimm_Summoner_Dream");
                    builder.AddItem(new MultiItem(item, new TermValue[]
                        {
                            new(loreTerm, 1),
                            new(summoner, 1)
                        }));
                }
                else
                    builder.AddItem(new SingleItem(item, new(loreTerm, 1)));
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
        }
        if (LoreRandomizer.RandoSettings.RandomizeShrineOfBelievers)
        {
            using Stream stream = ResourceHelper.LoadResource<LoreRandomizer>("Logic.ShrineLogic.json");
            builder.DeserializeJson(LogicManagerBuilder.JsonType.Locations, stream);

            // If grub tolerance is used, the internal counter is set to a negative value, making it impossible to reach 46.
            // Because of this, we set the max and half amount via the tolerance instead.
            int logicallyRequiredGrubs = 46 - settings.CostSettings.GrubTolerance;
            builder.DoLogicEdit(new("All_Grubs-Shrine", "(ORIG) + GRUBS>" + (logicallyRequiredGrubs - 1)));
            builder.DoLogicEdit(new("Half_Grub-Shrine", "(ORIG) + GRUBS>" + (logicallyRequiredGrubs / 2 - 1)));
        }
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
}
