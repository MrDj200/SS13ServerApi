using ByondTopic;
using System.Collections.Generic;
using System.Web;

namespace SS13ServerApi
{
    public class SS13Status
    {
        public string Version { get; set; }
        public string Mode { get; set; }
        public string Respawn { get; set; }
        public string Enter { get; set; }
        public int AI { get; set; }
        public int Players { get; set; }
        public string StationName { get; set; }
        public int ShuttleTime { get; set; }
        public int Elapsed { get; set; }
        public List<string> PlayerList { get; set; } = new List<string>();
        public string MapName { get; set; }

        public SS13Status(QueryResponse query)
        {
            var parsedQuery = HttpUtility.ParseQueryString(HttpUtility.HtmlDecode(query.ToString()));

            this.Version = parsedQuery["version"];
            this.Mode = parsedQuery["mode"];
            this.Respawn = parsedQuery["respawn"];
            this.Enter = parsedQuery["enter"];
            this.AI = Parse(parsedQuery["ai"]);
            this.Players = Parse(parsedQuery["players"]);
            this.StationName = parsedQuery["station_name"];
            this.ShuttleTime = Parse(parsedQuery["shuttle_time"]);
            this.Elapsed = Parse(parsedQuery["elapsed"]);
            this.MapName = parsedQuery["map_name"];
            FillPlayers(parsedQuery);
        }

        private void FillPlayers(System.Collections.Specialized.NameValueCollection parsedQuery)
        {
            foreach (string item in parsedQuery)
            {
                if (item != null && item.Contains("player") && item != "players")
                {
                    this.PlayerList.Add(parsedQuery[item]);
                }
            }
        }

        private int Parse(string msg) => int.TryParse(msg, out int temp) ? temp : -1;
    }
}
