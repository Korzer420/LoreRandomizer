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

    #endregion

    #region Methods

    public override void Initialize()
    {
        RandoInterop.Initialize();
        ConnectionMenu.AttachMenu();
        Instance = this;
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