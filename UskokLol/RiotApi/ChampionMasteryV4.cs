using Newtonsoft.Json.Linq;
using System;

public static class ChampionMasteryV4 
{
    public static Tuple<int, int> GetChampionMastery(Region reg, int championId, string summonerId) 
    {
        JToken token = Request.Execute<JToken>(reg, "champion-mastery/v4/champion-masteries/by-summoner/" + summonerId + "/by-champion/" + championId);
        Tuple<int, int> returnValue = new Tuple<int, int>((int)token["championPoints"], (int)token["championLevel"]);
        return returnValue;
    }
}