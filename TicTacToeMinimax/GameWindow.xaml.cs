using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameMode gameMode;
        private char player;
        private Difficulty difficulty;
        private Thread thread;
        private string currentGameState = "------------"; // 0-8 game, 9 - player turn, 10 - turn number, 11 - winner
        private bool isGameOver = false;
        private char winner = '/';
        private int indexClicked = -1;
        private bool btnClicked = false;

        public GameWindow(GameMode gameMode, char player, Difficulty difficulty)
        {
            InitializeComponent();
            this.gameMode = gameMode;
            this.player = player;
            this.difficulty = difficulty;
            Start();
        }

        private void Start()
        {
            switch (gameMode)
            {
                case GameMode.SINGLEPLAYER: 
                    thread = new Thread(new ThreadStart(SinglePlayerGame));
                    thread.Start();
                    break;

                case GameMode.MULTIPLAYER:
                    thread = new Thread(new ThreadStart(MultiPlayerGame));
                    thread.Start();
                    break;
            }
        }

        #region Singleplayer
        private void SinglePlayerGame()
        {
            ResetMoveNumber();
            if (player == 'X')
            {
                MinimaxAI minimaxAI = new MinimaxAI('O', player, difficulty);
                while (!isGameOver)
                {
                    SetTitleText("Your turn!");
                    EnableButtons();
                    SetButtonStates();
                    while (!isGameOver)
                    {
                        if (btnClicked)
                        {
                            IncrementNumberOfMoves();
                            SetTxtNumberOfMoves();
                            SetNewGameState(indexClicked, player);
                            CheckWinner();
                            if (isGameOver)
                            {
                                SetNewGameState(11, winner);
                                DisableButtons();
                                SetStatusText("You have won!");
                                SetTitleText("You have won!");
                                break;
                            }
                            else if (GetMoveNumber() >= 9)
                            {
                                isGameOver = true;
                                SetStatusText("Number of moves is 9.");
                                SetTitleText("Game over! It's a tie.");
                                break;
                            }
                            btnClicked = false;
                            indexClicked = -1;
                            break;
                        }
                    }
                    if (isGameOver)
                    {
                        break;
                    }
                    DisableButtons();
                    int index = minimaxAI.BestMove(currentGameState);
                    if (index >= 0)
                    {
                        IncrementNumberOfMoves();
                        SetTxtNumberOfMoves();
                        SetNewGameState(index, 'O');
                        SetButtonStates();
                    }
                    CheckWinner();
                    if (isGameOver)
                    {
                        SetTitleText("Game over! Opponent has won.");
                        SetStatusText("Game over! Opponent has won.");
                        break;
                    }
                    else if (GetMoveNumber() >= 9)
                    {
                        isGameOver = true;
                        SetStatusText("Number of moves is 9.");
                        SetTitleText("Game over! It's a tie.");
                        break;
                    }
                }
                thread.Join();
            }
            else
            {
                MinimaxAI minimaxAI = new MinimaxAI('X', player, difficulty);
                while (!isGameOver)
                {
                    DisableButtons();
                    //int index = GetRandomPosition();
                    int index = minimaxAI.BestMove(currentGameState);
                    if (index >= 0)
                    {
                        IncrementNumberOfMoves();
                        SetTxtNumberOfMoves();
                        SetNewGameState(index, 'X');
                        SetButtonStates();
                    }
                    CheckWinner();
                    if (isGameOver)
                    {
                        SetTitleText("Game over! Opponent has won.");
                        SetStatusText("Game over! Opponent has won.");
                        break;
                    }
                    else if (GetMoveNumber() >= 9)
                    {
                        isGameOver = true;
                        SetStatusText("Number of moves is 9.");
                        SetTitleText("Game over! It's a tie.");
                        break;
                    }
                    SetTitleText("Your turn!");
                    EnableButtons();
                    SetButtonStates();
                    while (!isGameOver)
                    {
                        if (btnClicked)
                        {
                            IncrementNumberOfMoves();
                            SetTxtNumberOfMoves();
                            SetNewGameState(indexClicked, player);
                            CheckWinner();
                            if (isGameOver)
                            {
                                SetNewGameState(11, winner);
                                DisableButtons();
                                SetStatusText("You have won!");
                                SetTitleText("You have won!");
                                break;
                            }
                            else if (GetMoveNumber() >= 9)
                            {
                                isGameOver = true;
                                SetStatusText("Number of moves is 9.");
                                SetTitleText("Game over! It's a tie.");
                                break;
                            }
                            btnClicked = false;
                            indexClicked = -1;
                            break;
                        }
                    }
                    if (isGameOver)
                    {
                        break;
                    }
                }
                thread.Join();
            }
        }

        #endregion

        #region Multiplayer
        private void MultiPlayerGame()
        {
            ResetMoveNumber();

            while (!isGameOver)
            {
                player = 'X';

                SetStatusText("Player X's turn...");
                SetTitleText("Player X's turn...");

                while (!isGameOver)
                {
                    if (btnClicked)
                    {
                        IncrementNumberOfMoves();
                        SetTxtNumberOfMoves();
                        SetNewGameState(indexClicked, player);

                        CheckWinner();
                        if (isGameOver)
                        {
                            SetNewGameState(11, winner);
                            DisableButtons();
                            SetStatusText("Game over! Player X has won.");
                            SetTitleText("Game over! Player X has won.");
                            thread.Join();
                        }

                        if (GetMoveNumber() >= 9)
                        {
                            isGameOver = true;
                            SetStatusText("Number of moves is 9.");
                            SetTitleText("Game over! It's a tie.");
                            thread.Join();
                        }

                        btnClicked = false;
                        indexClicked = -1;
                        break;
                    }
                }

                player = 'O';

                SetStatusText("Player O's turn...");
                SetTitleText("Player O's turn...");
                btnClicked = false;
                while (!isGameOver)
                {
                    if (btnClicked)
                    {
                        IncrementNumberOfMoves();
                        SetTxtNumberOfMoves();
                        SetNewGameState(indexClicked, player);

                        if (GetMoveNumber() >= 9)
                        {
                            isGameOver = true;
                            SetStatusText("Number of moves is 9.");
                            SetTitleText("Game over! It's a tie.");
                            break;
                        }
                        else
                        {
                            CheckWinner();
                            if (isGameOver)
                            {
                                SetNewGameState(11, winner);
                                DisableButtons();
                                SetStatusText("Game over! Player O has won.");
                                SetTitleText("Game over! Player O has won.");
                                break;
                            }
                        }
                        btnClicked = false;
                        indexClicked = -1;
                        break;
                    }
                }                        
            }
            thread.Join();
        }
        #endregion

        #region Set UI Text

        void SetStatusText(string text)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                txtStatus.Text = text;
            });
        }

        void SetTitleText(string text)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                txtTitle.Text = text;
            });
        }

        #endregion

        #region Override Game State 

        void SetNewGameState(int index, char player)
        {
            char[] oldGameState = currentGameState.ToCharArray();
            oldGameState[index] = player;
            string newGameState = new string(oldGameState);
            currentGameState = newGameState;
        }

        #endregion

        #region Move Number

        void ResetMoveNumber()
        {
            try
            {
                char[] oldGameState = currentGameState.ToCharArray();
                oldGameState[10] = '0';
                string newGameState = new string(oldGameState);
                currentGameState = newGameState;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Napaka pri pridobivanju številke poteze.");
            }
        }

        void IncrementNumberOfMoves()
        {
            try
            {
                int numberOfMoves = Int32.Parse(currentGameState[10].ToString());
                numberOfMoves++;
                char[] oldGameState = currentGameState.ToCharArray();
                oldGameState[10] = numberOfMoves.ToString()[0];
                string newGameState = new string(oldGameState);
                currentGameState = newGameState;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Napaka pri pridobivanju številke poteze.");
            }
        }

        int GetMoveNumber()
        {
            char moveNumber = currentGameState[10];
            return int.Parse(moveNumber.ToString());
        }

        void SetTxtNumberOfMoves()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                txtTurnNumber.Text = "Number of moves: " + currentGameState[10];
            });
        }

        #endregion

        #region Winner
        void SetWinner(char winner)
        {
            char[] oldGameState = currentGameState.ToCharArray();
            oldGameState[11] = winner;
            string newGameState = new string(oldGameState);
            currentGameState = newGameState;
        }

        void CheckWinner()
        {
            if (currentGameState != null && currentGameState.Length >= 9)
            {
                if (currentGameState[0].ToString() == currentGameState[1].ToString() && currentGameState[1].ToString() == currentGameState[2].ToString() && currentGameState[0].ToString() != "-") // prva vrstica isti elementi
                {
                    winner = currentGameState[1];
                    SetWinner(winner);
                    isGameOver = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (player == currentGameState[11])
                        {
                            btn0_0.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn0_1.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn0_2.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                        }
                        else
                        {
                            btn0_0.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn0_1.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn0_2.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                        }
                    });
                }
                else if (currentGameState[3].ToString() == currentGameState[4].ToString() && currentGameState[3].ToString() == currentGameState[5].ToString() && currentGameState[3].ToString() != "-") // druga vrstica isti elementi
                {
                    winner = currentGameState[3];
                    SetWinner(winner);
                    isGameOver = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (player == currentGameState[11])
                        {
                            btn1_0.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn1_1.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn1_2.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                        }
                        else
                        {
                            btn1_0.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn1_1.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn1_2.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                        }
                    });
                }
                else if (currentGameState[6].ToString() == currentGameState[7].ToString() && currentGameState[6].ToString() == currentGameState[8].ToString() && currentGameState[6].ToString() != "-") // tretuji vrstici isti elementi
                {
                    winner = currentGameState[6];
                    SetWinner(winner);
                    isGameOver = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (player == currentGameState[11])
                        {
                            btn2_0.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn2_1.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn2_2.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                        }
                        else
                        {
                            btn2_0.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn2_1.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn2_2.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                        }
                    });
                }
                else if (currentGameState[0].ToString() == currentGameState[3].ToString() && currentGameState[0].ToString() == currentGameState[6].ToString() && currentGameState[0].ToString() != "-") // prvi stolpec isti elementi
                {
                    winner = currentGameState[0];
                    SetWinner(winner);
                    isGameOver = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (player == currentGameState[11])
                        {
                            btn0_0.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn1_0.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn2_0.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                        }
                        else
                        {
                            btn0_0.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn1_0.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn2_0.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                        }
                    });
                }
                else if (currentGameState[1].ToString() == currentGameState[4].ToString() && currentGameState[1].ToString() == currentGameState[7].ToString() && currentGameState[1].ToString() != "-") // drugi stolpec
                {
                    winner = currentGameState[1];
                    SetWinner(winner);
                    isGameOver = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (player == currentGameState[11])
                        {
                            btn0_1.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn1_1.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn2_1.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                        }
                        else
                        {
                            btn0_1.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn1_1.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn2_1.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                        }
                    });
                }
                else if (currentGameState[2].ToString() == currentGameState[5].ToString() && currentGameState[2].ToString() == currentGameState[8].ToString() && currentGameState[2].ToString() != "-") // tretji stolpec
                {
                    winner = currentGameState[2];
                    SetWinner(winner);
                    isGameOver = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (player == currentGameState[11])
                        {
                            btn0_2.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn1_2.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn2_2.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                        }
                        else
                        {
                            btn0_2.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn1_2.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn2_2.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                        }
                    });
                }
                else if (currentGameState[0].ToString() == currentGameState[4].ToString() && currentGameState[0].ToString() == currentGameState[8].ToString() && currentGameState[0].ToString() != "-") // prva diagonala
                {
                    winner = currentGameState[0];
                    SetWinner(winner);
                    isGameOver = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (player == currentGameState[11])
                        {
                            btn0_0.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn1_1.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn2_2.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                        }
                        else
                        {
                            btn0_0.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn1_1.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn2_2.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                        }
                    });
                }
                else if (currentGameState[2].ToString() == currentGameState[4].ToString() && currentGameState[2].ToString() == currentGameState[6].ToString() && currentGameState[2].ToString() != "-") // druga diagonala
                {
                    winner = currentGameState[2];
                    SetWinner(winner);
                    isGameOver = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (player == currentGameState[11])
                        {
                            btn0_2.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn1_1.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                            btn2_0.Background = new SolidColorBrush(Color.FromRgb(40, 173, 100));
                        }
                        else
                        {
                            btn0_2.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn1_1.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                            btn2_0.Background = new SolidColorBrush(Color.FromRgb(211, 62, 67));
                        }
                    });
                }
                else
                {
                    winner = '/';
                    isGameOver = false;
                }
            }
            else
            {
                winner = '/';
                isGameOver = false;
            }
        }

        #endregion

        #region Buttons

        void EnableButtons()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                btn0_0.IsEnabled = true;
                btn0_1.IsEnabled = true;
                btn0_2.IsEnabled = true;
                btn1_0.IsEnabled = true;
                btn1_1.IsEnabled = true;
                btn1_2.IsEnabled = true;
                btn2_0.IsEnabled = true;
                btn2_1.IsEnabled = true;
                btn2_2.IsEnabled = true;
            });
        }

        void DisableButtons()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                btn0_0.IsEnabled = false;
                btn0_1.IsEnabled = false;
                btn0_2.IsEnabled = false;
                btn1_0.IsEnabled = false;
                btn1_1.IsEnabled = false;
                btn1_2.IsEnabled = false;
                btn2_0.IsEnabled = false;
                btn2_1.IsEnabled = false;
                btn2_2.IsEnabled = false;
            });
        }

        void SetButtonStates()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                for (int i = 0; i < 9; i++)
                {
                    if (currentGameState != null && currentGameState[i] != '-')
                    {
                        switch (i)
                        {
                            case 0:
                                btn0_0.IsEnabled = false;
                                btn0_0.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn0_0.IsEnabled = false;
                                }
                                break;
                            case 1:
                                btn0_1.IsEnabled = false;
                                btn0_1.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn0_1.IsEnabled = false;
                                }
                                break;
                            case 2:
                                btn0_2.IsEnabled = false;
                                btn0_2.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn0_2.IsEnabled = false;
                                }
                                break;

                            case 3:
                                btn1_0.IsEnabled = false;
                                btn1_0.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn1_0.IsEnabled = false;
                                }
                                break;
                            case 4:
                                btn1_1.IsEnabled = false;
                                btn1_1.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn1_1.IsEnabled = false;
                                }
                                break;
                            case 5:
                                btn1_2.IsEnabled = false;
                                btn1_2.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn1_2.IsEnabled = false;
                                }
                                break;

                            case 6:
                                btn2_0.IsEnabled = false;
                                btn2_0.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn2_0.IsEnabled = false;
                                }
                                break;
                            case 7:
                                btn2_1.IsEnabled = false;
                                btn2_1.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn2_1.IsEnabled = false;
                                }
                                break;
                            case 8:
                                btn2_2.IsEnabled = false;
                                btn2_2.Content = currentGameState[i];
                                if (currentGameState[i] != player)
                                {
                                    btn2_2.IsEnabled = false;
                                }
                                break;
                        }
                    }
                }
            });
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btn0_0_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn0_0.Content = "O";
            }
            else
            {
                btn0_0.Content = "X";
            }
            btn0_0.IsEnabled = false;
            btnClicked = true;
            indexClicked = 0;
        }

        private void btn0_1_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn0_1.Content = "O";
            }
            else
            {
                btn0_1.Content = "X";
            }
            btn0_1.IsEnabled = false;
            btnClicked = true;
            indexClicked = 1;
        }

        private void btn0_2_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn0_2.Content = "O";
            }
            else
            {
                btn0_2.Content = "X";
            }
            btn0_2.IsEnabled = false;
            btnClicked = true;
            indexClicked = 2;
        }

        private void btn1_0_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn1_0.Content = "O";
            }
            else
            {
                btn1_0.Content = "X";
            }
            btn1_0.IsEnabled = false;
            btnClicked = true;
            indexClicked = 3;
        }

        private void btn1_1_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn1_1.Content = "O";
            }
            else
            {
                btn1_1.Content = "X";
            }
            btn1_1.IsEnabled = false;
            btnClicked = true;
            indexClicked = 4;
        }

        private void btn1_2_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn1_2.Content = "O";
            }
            else
            {
                btn1_2.Content = "X";
            }
            btn1_2.IsEnabled = false;
            btnClicked = true;
            indexClicked = 5;
        }

        private void btn2_0_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn2_0.Content = "O";
            }
            else
            {
                btn2_0.Content = "X";
            }
            btn2_0.IsEnabled = false;
            btnClicked = true;
            indexClicked = 6;
        }

        private void btn2_1_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn2_1.Content = "O";
            }
            else
            {
                btn2_1.Content = "X";
            }
            btn2_1.IsEnabled = false;
            btnClicked = true;
            indexClicked = 7;
        }

        private void btn2_2_Click(object sender, RoutedEventArgs e)
        {
            if (player == 'O')
            {
                btn2_2.Content = "O";
            }
            else
            {
                btn2_2.Content = "X";
            }
            btn2_2.IsEnabled = false;
            btnClicked = true;
            indexClicked = 8;
        }
    }

    #endregion
}
