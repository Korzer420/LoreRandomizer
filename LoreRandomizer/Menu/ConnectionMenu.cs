using KorzUtils.Helper;
using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using System;
using System.Linq;

namespace LoreRandomizer.Menu;

public class ConnectionMenu
{
    #region Members

    private static ConnectionMenu _instance;

    private MenuPage _mainPage;

    private SmallButton _pageButton;

    private MenuElementFactory<RandoSettings> _menuElementFactory;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance of the menu.
    /// </summary>
    public static ConnectionMenu Instance => _instance ??= new();

    #endregion

    #region Event handler

    /// <summary>
    /// Check if it is possible to randomize Elderbug rewards.
    /// </summary>
    /// <param name="element"></param>
    private void CheckIfPossible(IValueElement element)
    {
        if ((bool)element.Value)
        {
            // For simplicity, three of the four pools have to be enabled for Elderbug to be possible.
            int poolsActive = 0;
            for (int i = 2; i < 6; i++)
                if ((bool)_menuElementFactory.Elements[i].Value)
                    poolsActive++;
            if (poolsActive < 3)
                element.SetValue(false);
        }
    }

    private void CheckIfElderbugPossible(IValueElement element)
    {
        if (!(bool)element.Value && (bool)_menuElementFactory.ElementLookup[nameof(RandoSettings.RandomizeElderbugRewards)].Value)
        {
            // For simplicity, three of the four pools have to be enabled for Elderbug to be possible.
            int poolsActive = 0;
            for (int i = 1; i < 6; i++)
                if ((bool)_menuElementFactory.Elements[i].Value)
                    poolsActive++;
            if (poolsActive < 1)
                _menuElementFactory.ElementLookup[nameof(RandoSettings.RandomizeElderbugRewards)].SetValue(false);
        }
    }

    #endregion

    #region Setup

    public static void AttachMenu()
    {
        RandomizerMenuAPI.AddMenuPage(Instance.ConstructMenu, Instance.HandleButton);
        MenuChangerMod.OnExitMainMenu += () => _instance = null;
    }

    private bool HandleButton(MenuPage previousPage, out SmallButton button)
    {
        try
        {
            _pageButton = new(previousPage, "Lore Rando");
            _pageButton.AddHideAndShowEvent(previousPage, _mainPage);
            _mainPage.BeforeGoBack += () => _pageButton.Text.color = !LoreRandomizer.Instance.Settings.Enabled ? Colors.DEFAULT_COLOR : Colors.TRUE_COLOR;
            _pageButton.Text.color = !LoreRandomizer.Instance.Settings.Enabled ? Colors.DEFAULT_COLOR : Colors.TRUE_COLOR;
            button = _pageButton;
            return true;
        }
        catch (Exception error)
        {
            LogHelper.Write<LoreRandomizer>("Failed to create connection button: ", error, false);
        }
        button = null;
        return true;
    }

    private void ConstructMenu(MenuPage previousPage)
    {
        try
        {
            _mainPage = new("Lore Rando", previousPage);
            _menuElementFactory = new(_mainPage, LoreRandomizer.Instance.Settings);
            new VerticalItemPanel(_mainPage, new(0f, 450f), 80f, true, _menuElementFactory.Elements.Take(_menuElementFactory.Elements.Length - 2).ToArray());
            new GridItemPanel(_mainPage, new(0, -350f), 2, 0, 400, false, _menuElementFactory.Elements.Skip(_menuElementFactory.Elements.Length - 2).ToArray());
            for (int i = 2; i < 6; i++)
                _menuElementFactory.Elements[i].SelfChanged += CheckIfElderbugPossible;
            if (!LoreRandomizer.Instance.Settings.RandomizeTravellerDialogues)
                _menuElementFactory.ElementLookup[nameof(RandoSettings.TravellerOrder)].Hide();
            _menuElementFactory.ElementLookup[nameof(RandoSettings.RandomizeTravellerDialogues)].SelfChanged += x =>
            {
                if ((bool)x.Value)
                    _menuElementFactory.ElementLookup[nameof(RandoSettings.TravellerOrder)].Show();
                else
                    _menuElementFactory.ElementLookup[nameof(RandoSettings.TravellerOrder)].Hide();
            };
            _menuElementFactory.ElementLookup[nameof(RandoSettings.RandomizeElderbugRewards)].SelfChanged += CheckIfPossible;
        }
        catch (Exception exception)
        {
            LogHelper.Write<LoreRandomizer>("Failed to construct connection menu: ",exception, false);
        }
    }

    #endregion

    #region Interop

    // To do: RSM
    internal void PassRSMSettings(RandoSettings settings)
    {
        if (settings == null)
            _menuElementFactory.ElementLookup[nameof(LoreRandomizer.RandoSettings.Enabled)].SetValue(false);
        else
            _menuElementFactory.SetMenuValues(settings);
    }

    #endregion
}
