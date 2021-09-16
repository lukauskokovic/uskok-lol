using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UskokLol.RiotApi;
using System.Linq;

namespace UskokLol.CustomControls
{
    public partial class ChampSelectPlayerInfo : UserControl
    {
        string lastGameId = "";
        long lastChampionId = -1;
        Color MaxWinrate = Color.FromRgb(255, 0, 0), MinWinrate = Color.FromRgb(0, 0, 0);
        SummonerDTO summonerDTO;

        public ChampSelectPlayerInfo()
        {
            InitializeComponent();
        }

        void HideComponents()
        {
            MasteryLevelEllipse.Visibility = Visibility.Hidden;
            MasteryPointsLabel.Visibility = Visibility.Hidden;
            RankedInfoTextBox.Visibility = Visibility.Hidden;
            PlayerRankLabel.Visibility = Visibility.Hidden;
        }

        public void SetPlayer(SessionPlayer sessionSummoner, ClientSummoner clientSummoner)
        {
            if(lastGameId != MainWindow.gameId)
            {
                HideComponents();
                lastGameId = MainWindow.gameId;

                summonerDTO = SummonerV4.SummonerByName(ClientAPI.PlayerRegion, clientSummoner.displayName);
                PlayerNameText.Content = summonerDTO.name;
                string positionUrl = "https://raw.communitydragon.org/latest/plugins/rcp-fe-lol-parties/global/default/icon-position-" + sessionSummoner.assignedPosition + "-red.png";
                set_ellipse_image(PlayerPositionEllipse, positionUrl);

                LeagueEntryDTO[] leagues = LeagueV4.GetSummonerLeagues(ClientAPI.PlayerRegion, summonerDTO.id);
                if(leagues.Any(x => x.queueType == "RANKED_SOLO_5x5"))
                {
                    RankedInfoTextBox.Visibility = Visibility.Visible;
                    PlayerRankLabel.Visibility = Visibility.Visible;

                    LeagueEntryDTO league = leagues.First(x => x.queueType == "RANKED_SOLO_5x5");
                    

                    PlayerRankLabel.Content = league.tier + " " + league.rank;
                    PlayerRankLabel.Foreground = new SolidColorBrush(get_rank_color(league.tier));
                    LPLabel.Text = league.leaguePoints + " LP";
                    WinsLabel.Text = league.wins.ToString();
                    LossesLabel.Text = league.losses.ToString();
                    float winRate = (float)league.wins / (float)(league.losses + league.wins);
                    winRate *= 100;
                    WinRateLabel.Text = winRate.ToString("N1") + "%";
                }
                
                
            }
            long champid = 0;
            int opacity = 100;
            if(sessionSummoner.championPickIntent != 0)
            {
                champid = sessionSummoner.championPickIntent;
                opacity = 75;
            }
            else if(sessionSummoner.championId != 0) champid = sessionSummoner.championId;

            PlayerChampionEllipse.Opacity = opacity;

            if (champid != 0 && champid != lastChampionId)
            {
                string championUrl = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-icons/" + champid + ".png";
                Clipboard.SetText(championUrl);
                set_ellipse_image(PlayerChampionEllipse, championUrl);
                lastChampionId = champid;
                try
                {
                    Tuple<int, int> points_level = ChampionMasteryV4.GetChampionMastery(ClientAPI.PlayerRegion, (int)champid, summonerDTO.id);
                    string masteryUrl = "https://raw.communitydragon.org/latest/plugins/rcp-fe-lol-profiles/global/default/mastery_level" + points_level.Item2 + ".png";
                    set_ellipse_image(MasteryLevelEllipse, masteryUrl);
                    MasteryLevelEllipse.Visibility = Visibility.Visible;
                    MasteryPointsLabel.Visibility = Visibility.Visible;
                    MasteryPointsLabel.Content = string.Format("{0:n0}", points_level.Item1);
                }
                catch
                {
                    MasteryLevelEllipse.Visibility = Visibility.Hidden;
                    MasteryPointsLabel.Visibility = Visibility.Hidden;
                }
            }
        }

        Color find_middle_color(Color Max, Color Min, float percent, float minPercent = 0f)
        {
            return Color.FromRgb(0, 0, 0);
        }

        void set_ellipse_image(Ellipse ellipse, string url)
        {
            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute)))
            {
                Stretch = Stretch.Fill
            };
            ellipse.Fill = brush;
        }

        Color get_rank_color(string rank)
        {
            return rank switch
            {
                "GOLD" => Color.FromRgb(220, 211, 0),
                "IRON" => Color.FromRgb(138, 138, 138),
                "PLATINUM" => Color.FromRgb(42, 221, 245),
                "SILVER" => Color.FromRgb(174, 176, 176),
                "BRONZE" => Color.FromRgb(255, 144, 33),
                "DIAMOND" => Color.FromRgb(36, 255, 244),
                "MASTER" => Color.FromRgb(225, 77, 255),
                "GRANDMASTER" => Color.FromRgb(124, 4, 204),
                "CHALLANGER" => Color.FromRgb(249, 255, 128),
                _ => Color.FromRgb(255, 255, 255),
            };
        }
    }
}
