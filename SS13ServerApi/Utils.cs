using ByondTopic;
using System.Text.Json;

namespace SS13ServerApi
{
    public static class Utils
    {
        public static string ProperJson(this QueryResponse query) => JsonSerializer.Serialize(new SS13Status(query));
    }
}
