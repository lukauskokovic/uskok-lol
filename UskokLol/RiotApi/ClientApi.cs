using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace UskokLol.RiotApi
{
    public static class ClientAPI
    {
        public static int Port = -1;
        public static Region PlayerRegion = Region.Eune;
        public static string AuthToken;
        public static string LocalPlayerName;
        public static void Load()
        {
            //wmic PROCESS WHERE name='LeagueClientUx.exe'
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "wmic",
                Arguments = @"/OUTPUT:STDOUT PROCESS WHERE name='LeagueClientUx.exe'",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            process.Start();
            string error = process.StandardError.ReadToEnd();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            if (error.Length == 0)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };// Ignores ssl error

                Match match = Regex.Match(output, @"--app-port=([0-9]*)");
                Port = int.Parse(match.Value.Replace("--app-port=", ""));

                match = Regex.Match(output, @"--region=([\w-_]*)");
                string region = match.Value.Replace("--region=", "");

                match = Regex.Match(output, @"--remoting-auth-token=([\w-_]*)");
                AuthToken = match.Value.Replace("--remoting-auth-token=", "");
                switch (region)
                {
                    case "EUW":
                        PlayerRegion = Region.Euw;
                        break;
                    case "EUNE":
                        PlayerRegion = Region.Eune;
                        break;
                }
                JObject obj = Request<JObject>("lol-summoner/v1/current-summoner");
                LocalPlayerName = (string)obj["displayName"];
            }
            else Port = -1;
        }

        public static T Request<T>(string endpoint)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("riot", AuthToken);
            string url = string.Format("https://127.0.0.1:{0}/{1}", Port, endpoint);
            return JsonConvert.DeserializeObject<T>(client.DownloadString(url));
        }
    }

    public class ClientApiChampSelectPlayer
    {
        public string assignedPosition;
        public long championId, summonerId;
    }
}
