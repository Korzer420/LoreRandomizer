using ItemChanger;
using RandomizerMod.RC;

namespace LoreRandomizer;

internal static class Extensions
{
    public static void AddList(this RequestBuilder builder, string[] items, string[] locations)
    {
        foreach (string item in items)
        { 
            builder.AddItemByName(item);
            builder.EditItemRequest(item, info =>
            {
                info.getItemDef = () => new()
                {
                    Name = item,
                    Pool = "Lore",
                    PriceCap = 1,
                    MajorItem = false
                };
            });
        }
        foreach (string location in locations)
        { 
            builder.AddLocationByName(location);
            builder.EditLocationRequest(location, info =>
            {
                info.getLocationDef = () => new()
                {
                    Name = location,
                    SceneName = Finder.GetLocation(location).sceneName,
                    FlexibleCount = false,
                    AdditionalProgressionPenalty = false
                };
            });
        }
    }
}
