using ByondTopic;
using ByondTopic.Response;
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
        public int? Vote { get; set; }
        public int? AI { get; set; }
        public string Host { get; set; }
        public string RoundID { get; set; }
        public int? Players { get; set; }
        public string Revision { get; set; }
        public string RevisionDate { get; set; }
        public int? HUB { get; set; }
        public int? Admins { get; set; }
        public int? GameState { get; set; }
        public string StationName { get; set; }
        public string ShuttleMode { get; set; }
        public int? ShuttleTime { get; set; }
        public int? Elapsed { get; set; }
        public List<string> PlayerList { get; set; } = new List<string>();
        public string MapName { get; set; }

        public SS13Status(TextQueryResponse query)
        {
            var parsedQuery = HttpUtility.ParseQueryString(HttpUtility.HtmlDecode(query.ToString()));

            this.Version = parsedQuery["version"];
            this.Mode = parsedQuery["mode"];
            this.Respawn = parsedQuery["respawn"];
            this.Enter = parsedQuery["enter"];
            this.Vote = Parse(parsedQuery["vote"]);
            this.AI = Parse(parsedQuery["ai"]);
            this.Host = parsedQuery["host"];
            this.RoundID = parsedQuery["round_id"];
            this.Players = Parse(parsedQuery["players"]);
            this.Revision = parsedQuery["revision"];
            this.RevisionDate = parsedQuery["revision_date"];
            this.HUB = Parse(parsedQuery["hub"]);
            this.Admins = Parse(parsedQuery["admins"]);
            this.GameState = Parse(parsedQuery["gamestate"]);
            this.StationName = parsedQuery["station_name"];
            this.ShuttleMode = parsedQuery["shuttle_mode"];
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

        private int? Parse(string msg) => int.TryParse(msg, out int temp) ? temp : (int?)null;
    }
}
