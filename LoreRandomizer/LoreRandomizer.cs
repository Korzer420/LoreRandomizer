using ItemChanger;
using LoreCore.Enums;
using LoreCore.Modules;
using LoreRandomizer.Menu;
using LoreRandomizer.ModInterop.ExtraRando;
using LoreRandomizer.RandoSetup;
using LoreRandomizer.SaveManagement;
using Modding;
using System.Linq;

namespace LoreRandomizer;

public class LoreRandomizer : Mod, IGlobalSettings<GlobalSaveData>
{
    #region Properties

    public static LoreRandomizer Instance { get; set; }

    public RandoSettings Settings { get; set; } = new();

    public static RandoSettings RandoSettings => Instance.Settings;

    public bool GenerateLoreTablets { get; set; }

    public override string GetVersion() => "0.4.1.0";

    #endregion

    #region Methods

    public override void Initialize()
    {
        Instance = this;
        ConnectionMenu.AttachMenu();
        RandoInterop.Initialize();
        On.UIManager.StartNewGame += UIManager_StartNewGame;
        if (ModHooks.GetMod("ExtraRando") is Mod)
        {
            HookExtraRando();
        }
    }

    private void UIManager_StartNewGame(On.UIManager.orig_StartNewGame orig, UIManager self, bool permaDeath, bool bossRush)
    {
        orig(self, permaDeath, bossRush);
        if (GenerateLoreTablets)
            LoreCore.LoreCore.Instance.CreateVanillaCustomLore();
        if (RandomizerMod.RandomizerMod.IsRandoSave && RandoSettings.Enabled
            && RandoSettings.RandomizeTravellerDialogues && RandoSettings.TravellerOrder == TravellerBehaviour.None)
        {
            TravellerControlModule module = ItemChangerMod.Modules.GetOrAdd<TravellerControlModule>();
            foreach (Traveller traveller in module.Stages.Keys.ToList())
                module.Stages[traveller] = 10;
        }
    }

    public void OnLoadGlobal(GlobalSaveData s)
    {
        if (s == null || s.RandomizerSettings == null)
            return;
        Settings = s.RandomizerSettings;
    }

    public GlobalSaveData OnSaveGlobal()
    {
        return new()
        {
           RandomizerSettings = Settings
        };
    }

    private void HookExtraRando()
    {
        ExtraRando.ModInterop.ItemChangerInterop.Modules.VictoryModule.RequestConditions += conditionList =>
        { 
            conditionList.Add(new PreceptVictoryCondition());
            conditionList.Add(new LoreVictoryCondition());
        };
    }

    #endregion
}