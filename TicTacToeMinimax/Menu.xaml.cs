using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToeMinimax
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Menu: Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnMultiplayer_Click(object sender, RoutedEventArgs e)
        {
            OpenGameWindow(GameMode.MULTIPLAYER);
        }

        private void btnSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            OpenGameWindow(GameMode.SINGLEPLAYER);
        }

        private void OpenGameWindow(GameMode gameMode)
        {
            if (gameMode == GameMode.SINGLEPLAYER)
            {
                ChoosePlayer choosePlayer = new ChoosePlayer();
                choosePlayer.Show();
                this.Close();
            }
            else
            {
                GameWindow gameWindow = new GameWindow(gameMode, '-', Difficulty.NONE);
                gameWindow.Show();
                this.Close();
            }
        }
    }
}
