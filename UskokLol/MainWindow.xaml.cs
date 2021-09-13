using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UskokLol.RiotApi;
using System.Linq;
using UskokLol.CustomControls;

namespace UskokLol
{
    public partial class MainWindow : Window
    {
        ChampSelectPlayerInfo[] PlayerInfoControls = new ChampSelectPlayerInfo[5];
        public MainWindow()
        {
            InitializeComponent();
            ClientAPI.Load();
            SummonerName.Content = ClientAPI.LocalPlayerName;
            int i = 0;
            foreach(var uiControl in ChampSelectGrid.Children.OfType<ChampSelectPlayerInfo>())
                PlayerInfoControls[i++] = uiControl;
            
        }

        void LoadChampSelect()
        {
            JToken Session = null;
            try
            {
                Session = ClientAPI.Request<JToken>("lol-champ-select/v1/session");
            }
            catch
            {
                MessageBox.Show("Not in champ select");
                return;
            }
            ClientApiChampSelectPlayer[] PlayersChampSelect = Session["myTeam"].Children().Select(x => x.ToObject<ClientApiChampSelectPlayer>()).ToArray();
            ClientApiSummoner[] Summoners = PlayersChampSelect.Select(x => ClientAPI.Request<ClientApiSummoner>("lol-summoner/v1/summoners/" + x.summonerId)).ToArray();
            for (int i = 0; i < Summoners.Length; i++)
            {
                SummonerDTO Summoner = SummonerV4.SummonerByName(ClientAPI.PlayerRegion, Summoners[i].displayName);
                PlayerInfoControls[i].SetPlayer(Summoner, PlayersChampSelect[i]);//ClientApiChampSelectPlayer
            }
        }

        #region Events
        private void HeaderBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void MinimizeBorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.WindowState = WindowState.Minimized;
        }
        private void ExitBorderMouseDown(object sender, MouseButtonEventArgs e) => Environment.Exit(0);
        private void BorderMouseEnter(object sender, MouseEventArgs e) => set_border_alpha(sender, 150);
        private void BorderMouseLeave(object sender, MouseEventArgs e) => set_border_alpha(sender, 255);
        void set_border_alpha(object border, byte alpha)
        {
            Border send = (Border)border;
            SolidColorBrush brush = send.Background as SolidColorBrush;
            brush.Color = Color.FromArgb(alpha, brush.Color.R, brush.Color.G, brush.Color.B);
            send.Background = brush;
        }
        #endregion

        private void ChampionSelectButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 1)
                LoadChampSelect();
        }
    }
}
