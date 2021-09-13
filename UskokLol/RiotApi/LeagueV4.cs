public static class LeagueV4 
{
    public static LeagueEntryDTO[] GetSummonerLeagues(Region region, string summonerId)
    {
        return Request.Execute<LeagueEntryDTO[]>(region, "league/v4/entries/by-summoner/" + summonerId);
    }
}

public class LeagueEntryDTO
{
    public string leagueId;
    public string summonerId;
    public string summonerName;
    public string queueType;
    public string tier;
    public string rank;
    public int leaguePoints;
    public int wins;
    public int losses;
    public bool hotStreak;
    public bool veteran;
    public bool freshBlood;
    public bool inactive;
    public MiniSeriesDTO miniSeries;
}
public class MiniSeriesDTO {
    public int losses;
    public string progress;
    public int target;
    public int wins;
}