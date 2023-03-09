using RandomizerMod.RC;

namespace LoreRandomizer;

internal static class Extensions
{
    public static void AddList(this RequestBuilder builder, string[] items, string[] locations)
    {
        foreach (string item in items)
            builder.AddItemByName(item);
        foreach (string location in locations)
            builder.AddLocationByName(location);
    }
}
