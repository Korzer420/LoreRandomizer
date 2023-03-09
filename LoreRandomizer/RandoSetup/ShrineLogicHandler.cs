using ItemChanger;
using KorzUtils.Helper;
using Newtonsoft.Json;
using RandomizerCore.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoreRandomizer.RandoSetup;

internal static class ShrineLogicHandler
{
    #region Properties

    public static Dictionary<string, LogicDef> LogicDefs { get; set; } = new();

    #endregion

    #region Methods

    internal static void Initialize()
    {
        List<LogicDef> logicDefs = ResourceHelper.LoadJsonResource<LoreRandomizer, List<LogicDef>>("ShrineLogic.json");
        LogicDefs = logicDefs.ToDictionary(x => x.Name, x => x);
    }

    #endregion
}
