using Newtonsoft.Json;
using System.Net;

public static class Request 
{
    public static string APIKEY = "RGAPI-3bb0015c-5bee-4814-b425-068319df247f";
    public static T Execute<T>(Region region, string endpoint, string extraData = "")
    {
        WebClient client = new WebClient();
        string response = client.DownloadString(string.Format("https://{0}.api.riotgames.com/lol/{1}?{2}api_key={3}", get_region_request_name(region), endpoint, extraData, APIKEY));
        return JsonConvert.DeserializeObject<T>(response);
    }

    public static string GetRegionName(Region reg)
    {
        switch (reg) 
        {
            case Region.Eune:
                return "Eune";
            case Region.Euw:
                return "Eu West";
            case Region.Brazil:
                return "Brazil";
            case Region.Japan:
                return "Japan";
            case Region.Korea:
                return "Korea";
            case Region.LatinoAmerica1:
                return "Latino 1";
            case Region.LatinoAmerica2:
                return "Latino 2";
            case Region.NorthAmerica:
                return "America";
            case Region.Oceania:
                return "Ocenia";
            case Region.Russia:
                return "Russia";
            case Region.Trukey:
                return "Turkey";
        }
        return "null";
    }

    private static string get_region_request_name(Region reg)
    {
        switch (reg)
        {
            case Region.Eune:
                return "EUN1";
            case Region.Euw:
                return "EUW1";
            case Region.Brazil:
                return "BR1";
            case Region.Japan:
                return "JP1";
            case Region.Korea:
                return "KR";
            case Region.LatinoAmerica1:
                return "LA1";
            case Region.LatinoAmerica2:
                return "LA2";
            case Region.NorthAmerica:
                return "NA1";
            case Region.Oceania:
                return "OC1";
            case Region.Russia:
                return "RU";
            case Region.Trukey:
                return "TR1";
        }
        return "null";
    }
}