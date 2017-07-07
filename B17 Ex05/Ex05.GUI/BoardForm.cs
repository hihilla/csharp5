﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Ex05.Logic;

namespace Ex05.GUI
{
    public class BoardForm : Form
    {
        private readonly int r_NumberOfRounds;
        private const int k_NumberOfButtonsInGuess = 4;
        private List<List<GuessButton>> m_GuessRows;
        private List<GuessButton> m_ArrowButtons;
        private List<List<Button>> m_ScoreButtons;
        private int m_CurrentRound = 0;
        private Game m_CurrentGame;

        public int NumberOfButtonsInGuess
        {
            get
            {
                return k_NumberOfButtonsInGuess;
            }
        }

        public BoardForm(int i_NumberOfRounds) : base()
        {
            r_NumberOfRounds = i_NumberOfRounds;
            this.Size = new Size(300, 80 + (50 * i_NumberOfRounds));
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Bullseye";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initControls();
            setControlsEventHandlers();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            initControls();
            setControlsEventHandlers();
        }

        private void initControls()
        {
            List<Control> controls = new List<Control>();
            for (int i = 0; i < k_NumberOfButtonsInGuess; i++)
            {
                Button blackButton = new SequenceButton();
                blackButton.Location = new Point(15 + (45 * i), 15);
                controls.Add(blackButton);
            }

            for (int row = 0; row < r_NumberOfRounds; row++)
            {
                List<GuessButton> buttonsInRow = new List<GuessButton>();
                for (int colum = 0; colum < k_NumberOfButtonsInGuess; colum++)
                {
                    GuessButton grayButton = new GuessButton(row);
                    grayButton.Location = new Point(15 + (45 * colum), 80 + (45 * row));
                    buttonsInRow.Add(grayButton);
                    controls.Add(grayButton);
                }
                m_GuessRows.Add(buttonsInRow);
                m_ArrowButtons.Add(generateArrowButton(row));
                m_ScoreButtons.Add(generateScoreButtons(row));
                controls.Add(m_ArrowButtons[row]);
                controls.AddRange(m_ScoreButtons[row].ToArray());
            }

            this.Controls.AddRange(controls.ToArray());
        }

        private GuessButton generateArrowButton(int i_RowNumber)
        {
            GuessButton arrowButton = new GuessButton(i_RowNumber);
            arrowButton.Size = new Size(40, 20);
            arrowButton.Location = new Point(195, 90 + (45 * i_RowNumber));
            arrowButton.Text = "-->>";
            arrowButton.TextAlign = ContentAlignment.MiddleCenter;
            arrowButton.Enabled = false;
            return arrowButton;
        }

        private List<Button> generateScoreButtons(int i_RowNumber)
        {
            List<Button> scoreButtons = new List<Button>();
            for (int i = 0; i < k_NumberOfButtonsInGuess; i++)
            {
                GuessButton scoreButton = new GuessButton(i_RowNumber);
                scoreButton.Size = new Size(15, 15);
                scoreButton.Location = new Point(this.ClientSize.Width - scoreButton.Width - 10 - (20 * (i % 2)),
                                                 80 + (20 * (i / 2) + (45 * i_RowNumber)));
                scoreButtons.Add(scoreButton);
            }

            return scoreButtons;
        }

        private void setControlsEventHandlers()
        {
            setArrowBottonsOnClicks();
            setGuessButtonsOnClicks();
        }

        private void setGuessButtonsOnClicks()
        {

        }

        private void setArrowBottonsOnClicks()
        {

        }

        private void guessButton_Click(object sender, EventArgs e)
        {
            // Show colors
        }

        private void arrowButton_Click(object sender, EventArgs e)
        {
            int correctInPlace = 0;
            int correctMissPlaced = 0;

            // get guess from colors window and send as List<char> to game.FeedbackForPlayerGuess (with out params)
            GuessButton arrowButton = sender as GuessButton;
            if (arrowButton != null)
            {
                showScore(arrowButton.Row, correctInPlace, correctMissPlaced);
            }
        }

        private List<char> getCurrentGuess()
        {
            List<char> guess = new List<char>();
            for (int i = 0; i < k_NumberOfButtonsInGuess; i++) {
                guess.Add(m_GuessRows[m_CurrentRound][i].Guess);
            }
            return guess;
        }

        public void ActivateRow(int i_RowNumber)
        {
            this.m_CurrentRound = i_RowNumber;
            for (int i = 0; i < k_NumberOfButtonsInGuess; i++)
            {
                m_GuessRows[i_RowNumber][i].Enabled = true;
            }
            m_ArrowButtons[i_RowNumber].Enabled = true;
        }

        private void showScore(int i_Row, int i_CorrectInPlace, int i_CorrectMissPlace)
        {
            for (int bull = 0; bull < i_CorrectInPlace; bull++)
            {
                m_ScoreButtons[i_Row][bull].BackColor = Color.Black;
            }

            for (int p = 0; p < i_CorrectMissPlace && p + i_CorrectInPlace <= 4; p++)
            {
                m_ScoreButtons[i_Row][p + i_CorrectInPlace].BackColor = Color.Black;
            }
        }

    }

    public class SequenceButton : Button
    {
        public SequenceButton() : base()
        {
            this.BackColor = Color.Black;
            this.Size = new Size(40, 40);
            this.Enabled = false;
        }
    }

    public class GuessButton : SequenceButton
    {
        private int m_Row;
        private char m_Guess;

        public char Guess
        {
            get
            {
                return m_Guess;
            }
        }

        public int Row
        {
            get
            {
                return m_Row;
            }
        }

        //     private List<char> m_Guess;

        //     public List<char> Guess
        //     {
        //         get
        //         {
        //             return m_Guess;
        //         }
        //         set
        //         {
        //	List<char> userGuess = new List<char>();
        //	bool validGuess = false;

        //	while (!validGuess)
        //	{
        //		char currentInputLetter;
        //		int letterCounter = 0;
        //		bool validLetter = true;

        //                 for (int i = 0; i < value.Count && validLetter; i += 2)
        //		{
        //                     currentInputLetter = value[i];
        //			letterCounter++;

        //			if (userGuess.Contains(currentInputLetter))
        //			{
        //				throw new ArgumentException("Please use each letter only once");
        //			}
        //			else
        //			{
        //				userGuess.Add(currentInputLetter);
        //			}
        //		}

        //                 if (letterCounter != 4 || !validLetter)
        //		{
        //			validGuess = false;
        //			userGuess.Clear();
        //		}
        //		else
        //		{
        //			validGuess = true;
        //		}
        //	}

        //             m_Guess = userGuess;
        //}
        //}

        public GuessButton(int i_Row) : base()
        {
            m_Row = i_Row;
            this.BackColor = Color.Gray;
        }
    }
}