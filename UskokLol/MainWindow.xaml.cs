using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UskokLol.RiotApi;
using System.Linq;

namespace UskokLol
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClientAPI.Load();
            SummonerName.Content = ClientAPI.LocalPlayerName;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            JObject response = ClientAPI.Request<JObject>("lol-champ-select/v1/session");
            ClientApiChampSelectPlayer[] array = response["myTeam"].Children().Select(x => x.ToObject<ClientApiChampSelectPlayer>()).ToArray();
            
        }
    }
}
