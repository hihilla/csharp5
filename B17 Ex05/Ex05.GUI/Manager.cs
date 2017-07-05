using System.Collections.Generic;
using Ex05.Logic;

namespace Ex05.GUI
{
    public class Manager
    {
        private int m_PlayersNumberOfRounds;
        private int m_CurrentRound;
        private string m_GamesWord;
        private List<List<char>> m_ListOfPlayerGuesses = new List<List<char>>();
        private List<List<char>> m_ListOfGuessesFeedback = new List<List<char>>();
        private bool m_PlayerWins = false;
        private bool m_KeepPlaying = true;

        public Manager()
        {
        }

        public void Run()
        {
            m_KeepPlaying = true;

        }

        private void gameOn()
        {
			Game game = new Game();
			game.RandomizeNewWord();
			m_GamesWord = game.GetWord();
			Player player = new Player();

        }
    }
}
