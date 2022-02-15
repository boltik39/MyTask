using Newtonsoft.Json;

namespace Utility.Utils;

public static class ConfigWork
{
    private static readonly Dictionary<string, string>? ConfDict =
        JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("../../../Configs/config.json"));

    public static string GetFromConfig(string key)
    {
        return ConfDict?[key] ?? string.Empty;
    }
}