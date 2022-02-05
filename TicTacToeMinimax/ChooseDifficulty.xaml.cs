using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TicTacToeMinimax
{
    /// <summary>
    /// Interaction logic for ChooseDifficulty.xaml
    /// </summary>
    public partial class ChooseDifficulty : Window
    {
        private GameMode gameMode;
        private char player;

        public ChooseDifficulty(GameMode gameMode, char player)
        {
            InitializeComponent();
            this.gameMode = gameMode;
            this.player = player;
        }

        private void BtnEasy_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(gameMode, player, Difficulty.EASY);
            gameWindow.Show();
            this.Close();
        }

        private void BtnNormal_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(gameMode, player, Difficulty.NORMAL);
            gameWindow.Show();
            this.Close();
        }

        private void BtnHard_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(gameMode, player, Difficulty.HARD);
            gameWindow.Show();
            this.Close();
        }
    }
}
