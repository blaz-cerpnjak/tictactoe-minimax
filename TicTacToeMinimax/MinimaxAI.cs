using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeMinimax
{
    public class MinimaxAI
    {
        private char player;
        private char opponent;
        private int depth;

        public MinimaxAI(char player, char opponent, Difficulty difficulty)
        {
            this.player = player;
            this.opponent = opponent;

            if (difficulty == Difficulty.HARD)
            {
                depth = -1;
            } 
            else if (difficulty == Difficulty.NORMAL)
            {
                depth = 5;
            } else if (difficulty == Difficulty.EASY)
            {
                depth = 3;
            }
        }

        public int BestMove(string gameState)
        {
            int bestScore = int.MinValue;
            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                if (gameState[i] == '-')
                {
                    char[] oldGameState = gameState.ToCharArray();
                    oldGameState[i] = player;
                    gameState = new string(oldGameState);

                    int score = Minimax(gameState, 0, false);

                    oldGameState = gameState.ToCharArray();
                    oldGameState[i] = '-';
                    gameState = new string(oldGameState);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        index = i;
                    }
                }
            }
            return index;
        }

        private char CheckWinner(string gameStateCopy)
        {
            if (gameStateCopy[0].ToString() == gameStateCopy[1].ToString() && gameStateCopy[1].ToString() == gameStateCopy[2].ToString() && gameStateCopy[0].ToString() != "-") // prva vrstica isti elementi
            {
                return gameStateCopy[1];
            }
            else if (gameStateCopy[3].ToString() == gameStateCopy[4].ToString() && gameStateCopy[3].ToString() == gameStateCopy[5].ToString() && gameStateCopy[3].ToString() != "-") // druga vrstica isti elementi
            {
                return gameStateCopy[3];
            }
            else if (gameStateCopy[6].ToString() == gameStateCopy[7].ToString() && gameStateCopy[6].ToString() == gameStateCopy[8].ToString() && gameStateCopy[6].ToString() != "-") // tretuji vrstici isti elementi
            {
                return gameStateCopy[6];
            }
            else if (gameStateCopy[0].ToString() == gameStateCopy[3].ToString() && gameStateCopy[0].ToString() == gameStateCopy[6].ToString() && gameStateCopy[0].ToString() != "-") // prvi stolpec isti elementi
            {
                return gameStateCopy[0];
            }
            else if (gameStateCopy[1].ToString() == gameStateCopy[4].ToString() && gameStateCopy[1].ToString() == gameStateCopy[7].ToString() && gameStateCopy[1].ToString() != "-") // drugi stolpec
            {
                return gameStateCopy[1];
            }
            else if (gameStateCopy[2].ToString() == gameStateCopy[5].ToString() && gameStateCopy[2].ToString() == gameStateCopy[8].ToString() && gameStateCopy[2].ToString() != "-") // tretji stolpec
            {
                return gameStateCopy[2];
            }
            else if (gameStateCopy[0].ToString() == gameStateCopy[4].ToString() && gameStateCopy[0].ToString() == gameStateCopy[8].ToString() && gameStateCopy[0].ToString() != "-") // prva diagonala
            {
                return gameStateCopy[0];
            }
            else if (gameStateCopy[2].ToString() == gameStateCopy[4].ToString() && gameStateCopy[2].ToString() == gameStateCopy[6].ToString() && gameStateCopy[2].ToString() != "-") // druga diagonala
            {
                return gameStateCopy[2];
            }
            else
            {
                if (IsSpotsLeft(gameStateCopy))
                {
                    return '/';
                }
                return '-';
            }
        }

        private int GetScore(char player)
        {
            if (this.player == player)
            {
                return +10;
            }
            else if (this.opponent == player)
            {
                return -10;
            }
            else
            {
                return 0;
            }
        }

        private bool IsSpotsLeft(string gameState)
        {
            for (int i = 0; i < 9; i++)
            {
                if (gameState[i] == '-')
                {
                    return true;
                }
            }
            return false;
        }

        private int Minimax(string gameState, int depth, bool isMaximizing)
        {
            if (depth == this.depth)
            {
                return 0;
            }

            char winner = CheckWinner(gameState);
            if (winner != '/')
            {
                return GetScore(winner);
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < 9; i++)
                {
                    if (gameState[i] == '-')
                    {
                        char[] oldGameState = gameState.ToCharArray();
                        oldGameState[i] = player;
                        gameState = new string(oldGameState);

                        int score = Minimax(gameState, depth + 1, false);

                        oldGameState = gameState.ToCharArray();
                        oldGameState[i] = '-';
                        gameState = new string(oldGameState);

                        if (score > bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < 9; i++)
                {
                    if (gameState[i] == '-')
                    {
                        char[] oldGameState = gameState.ToCharArray();
                        oldGameState[i] = opponent;
                        gameState = new string(oldGameState);

                        int score = Minimax(gameState, depth + 1, true);

                        oldGameState = gameState.ToCharArray();
                        oldGameState[i] = '-';
                        gameState = new string(oldGameState);

                        if (score < bestScore)
                        {
                            bestScore = score;
                        }
                    }
                }
                return bestScore;
            }
        }
    }
}