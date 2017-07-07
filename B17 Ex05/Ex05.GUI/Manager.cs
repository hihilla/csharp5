﻿﻿﻿using System.Collections.Generic;
using Ex05.Logic;
using System;

namespace Ex05.GUI
{
    public class Manager
    {
        private string m_GamesWord;
        private List<List<char>> m_ListOfPlayerGuesses = new List<List<char>>();
        private List<List<char>> m_ListOfGuessesFeedback = new List<List<char>>();

        public void Run()
        {
            Game game = new Game();
            game.RandomizeNewWord();
            m_GamesWord = game.GetWord();
            GuessesForm guessForm = new GuessesForm();

            if (guessForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int numberOfGuesses = guessForm.CounterOfGuessesClicks;
                BoardForm board = new BoardForm(numberOfGuesses, game);
                board.ShowDialog();
            }
        }
    }
}