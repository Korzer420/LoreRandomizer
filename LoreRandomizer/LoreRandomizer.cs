using ItemChanger;
using LoreCore.Enums;
using LoreCore.Modules;
using LoreRandomizer.Menu;
using LoreRandomizer.RandoSetup;
using LoreRandomizer.SaveManagement;
using Modding;

namespace LoreRandomizer;

public class LoreRandomizer : Mod, IGlobalSettings<GlobalSaveData>
{
    #region Properties

    public static LoreRandomizer Instance { get; set; }

    public RandoSettings Settings { get; set; } = new();

    public static RandoSettings RandoSettings => Instance.Settings;

    public bool GenerateLoreTablets { get; set; }

    public override string GetVersion() => "0.3.0.0";

    #endregion

    #region Methods

    public override void Initialize()
    {
        Instance = this;
        ConnectionMenu.AttachMenu();
        RandoInterop.Initialize();
        On.GameManager.StartNewGame += GameManager_StartNewGame;
    }

    private void GameManager_StartNewGame(On.GameManager.orig_StartNewGame orig, GameManager self, bool permadeathMode, bool bossRushMode)
    {
        orig(self, permadeathMode, bossRushMode);
        if (GenerateLoreTablets)
            LoreCore.LoreCore.Instance.CreateVanillaCustomLore();
        if (RandomizerMod.RandomizerMod.IsRandoSave && RandoSettings.Enabled 
            && RandoSettings.RandomizeTravellerDialogues && RandoSettings.TravellerOrder == TravellerBehaviour.None)
        {
            TravellerControlModule module = ItemChangerMod.Modules.GetOrAdd<TravellerControlModule>();
            foreach (Traveller traveller in module.Stages.Keys)
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

    #endregion
}