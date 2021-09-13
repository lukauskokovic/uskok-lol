using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UskokLol.RiotApi;

namespace UskokLol.CustomControls
{
    public partial class ChampSelectPlayerInfo : UserControl
    { 
        public ChampSelectPlayerInfo()
        {
            InitializeComponent();
        }

        public void SetPlayer(SummonerDTO Summoner, ClientApiChampSelectPlayer player)
        {
            PlayerNameText.Content = Summoner.name;
            string positionUrl = "https://raw.communitydragon.org/latest/plugins/rcp-fe-lol-parties/global/default/icon-position-" + player.assignedPosition + "-red.png";
            set_ellipse_image(PlayerPositionEllipse, positionUrl);

            string championUrl = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-icons/" + player.championId + ".png";
            set_ellipse_image(PlayerChampionEllipse, championUrl);

            LeagueEntryDTO[] leagues = LeagueV4.GetSummonerLeagues(ClientAPI.PlayerRegion, Summoner.id);
            for(int i = 0; i < leagues.Length; i++)
            {
                if(leagues[i].queueType == "RANKED_SOLO_5x5") 
                {
                    PlayerRankText.Content = leagues[i].tier + " " + leagues[i].rank;
                }
            }
        }

        void set_ellipse_image(Ellipse ellipse, string url)
        {
            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute)))
            {
                Stretch = Stretch.Fill
            };
            ellipse.Fill = brush;
        }
    }
}
