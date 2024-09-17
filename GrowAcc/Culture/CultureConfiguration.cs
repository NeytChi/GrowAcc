using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace GrowAcc.Culture
{
    public static class CultureConfiguration
    {
        public static bool init = false;
        public static Dictionary<string, ResponseCulture> responses { get; set; }
        
        public static string DefineCulture(StringValues acceptedLanguages)
        {
            if (acceptedLanguages.Contains("en-US"))
                return "en-US";
            if (acceptedLanguages.Contains("ua-UA"))
                return "ua-UA";
            return "en_US";
        }
        public static void TurnOn()
        {
            var filePath = "response-culture.json";

            var jsonString = File.ReadAllText(filePath);

            responses = JsonSerializer.Deserialize<Dictionary<string, ResponseCulture>>(jsonString);

            /// Todo: Добратися до файлу з відповідями.
            init = true;
        }
        public static string Get(string key, string culture)
        {
            if (!init)
                TurnOn();

            var value = responses[key];

            switch (culture)
            {
                case "ua-UA": return value.Ua;
                case "en-US": return value.Eng;
                default: return value.Eng;
            }
        }
    }
}
