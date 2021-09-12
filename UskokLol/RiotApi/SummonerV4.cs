using System;
using System.Collections.Generic;
using System.Text;

namespace UskokLol.RiotApi
{
    public static class SummonerV4
    {
        public static SummonerDTO SummonerByName(Region region, string name) => Request.Execute<SummonerDTO>(region, "summoner/v4/summoners/by-name/" + name);
        public static SummonerDTO SummonerByPUUID(Region region, string puuid) => Request.Execute<SummonerDTO>(region, "summoner/v4/summoners/by-puuid/" + puuid);
        public static SummonerDTO SummonerByAccountID(Region region, string accountId) => Request.Execute<SummonerDTO>(region, "summoner/v4/summoners/by-account/" + accountId);
        public static SummonerDTO SummonerBySummonerID(Region region, string summonerId) => Request.Execute<SummonerDTO>(region, "summoner/v4/summoners/" + summonerId);
    }

    public class SummonerDTO
    {
        public string accountId;
        public int profileIconId;
        public long revisionDate;
        public string name;
        public string id;
        public string puuid;
        public long summonerLevel;
    }
}
