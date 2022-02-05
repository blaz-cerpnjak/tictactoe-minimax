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
    /// Interaction logic for ChoosePlayer.xaml
    /// </summary>
    public partial class ChoosePlayer : Window
    {
        public ChoosePlayer()
        {
            InitializeComponent();
        }

        private void BtnX_Click(object sender, RoutedEventArgs e)
        {
            ChooseDifficulty chooseDifficulty = new ChooseDifficulty(GameMode.SINGLEPLAYER, 'X');
            chooseDifficulty.Show();
            this.Close();
        }

        private void BtnO_Click(object sender, RoutedEventArgs e)
        {
            ChooseDifficulty chooseDifficulty = new ChooseDifficulty(GameMode.SINGLEPLAYER, 'O');
            chooseDifficulty.Show();
            this.Close();
        }
    }
}
