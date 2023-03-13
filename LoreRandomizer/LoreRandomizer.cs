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

    public override string GetVersion() => "0.1.1.0";

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