using System;
using System.Collections.Generic;
using System.Text;
using B17_Ex02_BullsEyeEngine;
namespace B17_Ex02_BullsEyeConsole
{
    public class Manager
    {
        private int m_PlayersNumberOfRounds;
        private int m_CurrentRound;
        private string m_GamesWord;
        private List<List<char>> m_ListOfPlayerGuesses = new List<List<char>>();
        private List<List<char>> m_ListOfGuessesFeedback = new List<List<char>>();
        private StringBuilder m_AllResaults = new StringBuilder();
        private bool m_PlayerWins = false;
        private bool m_KeepPlaying = true;
        private const string k_boardStatus = "Current board status:";
        private const string k_TopRow = "|Pins:    |Results:|";
        private const string k_Delimiter = "|=========|========|";
        private const string k_EmptyRow = "|         |        |";
        private const string k_FirstRow = "| # # # # |        |";
		private const string k_Lose = "No more guesses allowed. You Lost.";
		private const string k_AskForNewGame = "Would you like to start a new game? (Y/N)";

		public void Run()
		{
			m_KeepPlaying = true;

			while (m_KeepPlaying)
			{
				GameOn();
				if (m_PlayerWins)
				{
					string winSentence = string.Format("You guessed after {0} steps!",
													   m_CurrentRound);
					Console.WriteLine(winSentence);
				}
				else if (!m_KeepPlaying)
				{
					Console.WriteLine("Bye Bye");
					break;
				}
				else
				{
					Console.Write("The word is ");
					Console.WriteLine(m_GamesWord);
					Console.WriteLine(k_Lose);
				}

				Console.WriteLine(k_AskForNewGame);
				char answer;
				while (!char.TryParse(Console.ReadLine(), out answer))
				{
					Console.WriteLine("invalid input");
					Console.WriteLine(k_AskForNewGame);
				}

				m_KeepPlaying = char.ToUpper(answer) == 'Y';
				Console.Clear();
			}
		}

        public void GameOn()
        {
            Game game = new Game();
            game.RandomizeNewWord();
            m_GamesWord = game.GetWord();
            Player player = new Player();

            chooseNumberOfGuesses(player);
            m_PlayersNumberOfRounds = player.NumberOfRounds;
            PrintBoard();

            for (m_CurrentRound = 1; m_CurrentRound < m_PlayersNumberOfRounds + 1 && !player.QuiteGame && !m_PlayerWins; m_CurrentRound++)
            {
                List<char> userGuess = player.GuessWord();
                if (player.QuiteGame)
                {
                    m_KeepPlaying = false;
                    m_CurrentRound--;
                    return;
                }

                List<char> guessFeedback = game.FeedbackForPlayerGuess(userGuess);
                if (isWin(guessFeedback))
                {
                    m_PlayerWins = true;
                    m_CurrentRound--;
                }

                m_ListOfPlayerGuesses.Add(userGuess);
                m_ListOfGuessesFeedback.Add(guessFeedback);
                addRoundToResults(userGuess, guessFeedback);
                PrintBoard();
            }
        }

        private List<char> guessWord(Player i_Player)
        {
            List<char> userGuess = new List<char>();
            bool validGuess = false;

            while (!validGuess)
            {
                Console.WriteLine(string.Format("Please enter your next guess <{0} - {1}> or 'Q' to quite",
                                                       i_Player.FirstLetterPossible,
                                                       i_Player.LastLetterPossible));
                try
                {
                    userGuess = i_Player.GuessWord();
                    validGuess = true;
                }
                catch (Exception ex)
                {
                    validGuess = false;
                    Console.WriteLine(ex.Message);
                }
            }

            return userGuess;
        }

        public void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine(k_boardStatus);
            Console.WriteLine();
            Console.WriteLine(k_TopRow);
            Console.WriteLine(k_Delimiter);
            Console.WriteLine(k_FirstRow);
            Console.WriteLine(k_Delimiter);
            Console.Write(m_AllResaults.ToString());
            for (int i = m_CurrentRound; i < m_PlayersNumberOfRounds; i++)
            {
                Console.WriteLine(k_EmptyRow);
                Console.WriteLine(k_Delimiter);
            }
        }

        private void addRoundToResults(List<char> i_Guess, List<char> i_Feedback)
        {
            string kka = string.Format("| {0} {1} {2} {3} | {4} {5} {6} {7}|",
                                       i_Guess[0], i_Guess[1],
                                       i_Guess[2], i_Guess[3],
                                       i_Feedback[0], i_Feedback[1],
                                       i_Feedback[2], i_Feedback[3]);
            
            m_AllResaults.Append(kka);
            m_AllResaults.Append(Environment.NewLine);
            m_AllResaults.Append(k_Delimiter);
            m_AllResaults.Append(Environment.NewLine);
        }

        private bool isWin(List<char> i_Feedback)
        {
            bool wonTheGame = true;

            foreach (char letter in i_Feedback)
            {
                wonTheGame = wonTheGame && (letter == 'V');
            }

            return wonTheGame;
        }

        private void chooseNumberOfGuesses(Player i_Player)
        {
            bool isNumber = false;
            bool isInRange = false;
            do
            {
                System.Console.WriteLine("Please enter number of guesses...");
                int numberOfGuesses;
                string userInput = Console.ReadLine();
                isNumber = int.TryParse(userInput, out numberOfGuesses);
                isInRange = i_Player.IsNumOfRoundsInRange(numberOfGuesses);
                if (!isNumber)
                {
                    System.Console.WriteLine(string.Format("Please only use numbers ({0}-{1})",
                                                           i_Player.MinNumberOfRounds,
                                                           i_Player.MaxNumberOfRounds));
                }
                if (!isInRange)
                {
                    System.Console.WriteLine(string.Format("Number not in range ({0}-{1})",
                                                           i_Player.MinNumberOfRounds,
                                                           i_Player.MaxNumberOfRounds));
                }
            } while (!isNumber || !isInRange);
        }
    }
}
