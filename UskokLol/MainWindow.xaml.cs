using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UskokLol.RiotApi;
using System.Linq;
using UskokLol.CustomControls;
using System.Windows.Threading;

namespace UskokLol
{
    public partial class MainWindow : Window
    {
        ChampSelectPlayerInfo[] PlayerInfoControls = new ChampSelectPlayerInfo[5];
        public static string gameId = "";
        DispatcherTimer LoadSessionTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            ClientAPI.Load();

            ChampSelectGrid.Visibility = Visibility.Hidden;
            SummonerName.Content = ClientAPI.LocalPlayerName;
            PlayerInfoControls = ChampSelectGrid.Children.OfType<ChampSelectPlayerInfo>().ToArray();
            LoadSessionTimer.Interval = new TimeSpan(0, 0, 1);
            LoadSessionTimer.Tick += (x,y) => LoadChampSelect(true);
        }

        private void LoadChampSelect(bool TimerInvoked = false)
        {
            JToken Session = null;
            try
            {
                Session = ClientAPI.Request<JToken>("lol-champ-select/v1/session");
                for(int i = 0; i < 5; i++)
                {
                    PlayerInfoControls[i].Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                LoadSessionTimer.Stop();
                if (!TimerInvoked)
                    MessageBox.Show("Not in champ select");
                return;
            }
            ChampSelectGrid.Visibility = Visibility.Visible;
            gameId = Session["gameId"].ToString();
            SessionPlayer[] PlayersChampSelect = Session["myTeam"].Children().Select(x => x.ToObject<SessionPlayer>()).ToArray();
            ClientSummoner[] Summoners = PlayersChampSelect.Select(x => ClientAPI.Request<ClientSummoner>("lol-summoner/v1/summoners/" + x.summonerId)).ToArray();
            for (int i = 0; i < Summoners.Length; i++)
            {
                PlayerInfoControls[i].Visibility = Visibility.Visible;
                PlayerInfoControls[i].SetPlayer(PlayersChampSelect[i], Summoners[i]);
            }

            LoadSessionTimer.Start();
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
            if (e.ClickCount == 1)
                LoadChampSelect();
        }
    }
}
