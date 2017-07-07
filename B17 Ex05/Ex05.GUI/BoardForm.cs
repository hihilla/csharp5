using System;
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
        private List<List<GuessButton>> m_GuessBottonsRows = new List<List<GuessButton>>();
        private List<GuessButton> m_ArrowButtons = new List<GuessButton>();
        private List<List<Button>> m_ScoreButtons = new List<List<Button>>();
        private List<List<char>> m_PlayersGuesses = new List<List<char>>();
        private int m_CurrentRound = 0;
        private Game m_CurrentGame;

        public int NumberOfButtonsInGuess
        {
            get
            {
                return k_NumberOfButtonsInGuess;
            }
        }

        public BoardForm(int i_NumberOfRounds, Game i_Game) : base()
        {
            r_NumberOfRounds = i_NumberOfRounds;
            m_CurrentGame = i_Game;
            for (int i = 0; i < r_NumberOfRounds; i++)
            {
                m_PlayersGuesses.Add(new List<char>());
            }
            this.Size = new Size(300, 90 + (50 * i_NumberOfRounds));
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Text = "Bullseye";
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
                blackButton.Enabled = false;
                blackButton.Location = new Point(15 + (45 * i), 15);
                controls.Add(blackButton);
            }

            for (int row = 0; row < r_NumberOfRounds; row++)
            {
                List<GuessButton> buttonsInRow = new List<GuessButton>();
                for (int colum = 0; colum < k_NumberOfButtonsInGuess; colum++)
                {
                    GuessButton grayButton = new GuessButton(row, colum);
                    if (row != 0)
                    {
                        grayButton.Enabled = false;
                    }
                    grayButton.Location = new Point(15 + (45 * colum), 80 + (45 * row));
                    buttonsInRow.Add(grayButton);
                    controls.Add(grayButton);
                }

                m_GuessBottonsRows.Add(buttonsInRow);
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
                GuessButton scoreButton = new GuessButton(i_RowNumber, i);
                scoreButton.Enabled = false;
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
            foreach (List<GuessButton> buttons in m_GuessBottonsRows)
            {
                foreach (GuessButton grayButton in buttons)
                {
                    grayButton.Click += new EventHandler(guessButton_Click);
                }
            }
        }

        private void setArrowBottonsOnClicks()
        {
            foreach (GuessButton arrowButton in m_ArrowButtons)
            {
                arrowButton.Click += new EventHandler(arrowButton_Click);
            }
        }

        private void enableRow(int i_Row)
        {
            foreach (GuessButton grayButton in m_GuessBottonsRows[i_Row])
            {
                grayButton.Enabled = true;
            }
        }

        private void disableRow(int i_Row)
        {
            foreach (GuessButton grayButton in m_GuessBottonsRows[i_Row])
            {
                grayButton.Enabled = true;
            }
        }

        private void disableAllRows()
        {
            for (int i = 0; i < r_NumberOfRounds; i++)
            {
                disableRow(i);
            }
        }

        private void guessButton_Click(object sender, EventArgs e)
        {
            GuessButton guessButton = sender as GuessButton;
            if (guessButton != null)
            {
                PickAColorForm colorsForm = new PickAColorForm(guessButton.Row,
                                                               guessButton.Colum,
                                                               m_PlayersGuesses[guessButton.Row]);
                colorsForm.ShowDialog();
                //DialogResult dialogResult = colorsForm.ShowDialog();
                //Console.WriteLine(dialogResult);
                //if (dialogResult != DialogResult.Cancel)
                //{
                m_PlayersGuesses[guessButton.Row] = colorsForm.Guess;
                guessButton.BackColor = colorsForm.ChosenColor;
                guessButton.SetGuess = true;
                if (allRowSet(guessButton.Row))
                {
                    m_ArrowButtons[guessButton.Row].Enabled = true;
                }
                //}
            }
        }

        private void arrowButton_Click(object sender, EventArgs e)
        {
            int correctInPlace = 0;
            int correctMissPlaced = 0;
            GuessButton arrowButton = sender as GuessButton;

            if (arrowButton != null)
            {
                arrowButton.Enabled = false;
                List<char> currentGuess = m_PlayersGuesses[arrowButton.Row];
                this.m_CurrentGame.FeedbackForPlayerGuess(currentGuess,
                                                          out correctInPlace,
                                                          out correctMissPlaced);

                showScore(arrowButton.Row, correctInPlace, correctMissPlaced);
                if (correctInPlace == 4)
                {
                    disableAllRows();
                }
                else if (arrowButton.Row + 1 <= r_NumberOfRounds)
                {
                    enableRow(arrowButton.Row + 1);
                }
            }
        }

        private bool allRowSet(int i_Row)
        {
            bool isSet = true;
            foreach (GuessButton guessButton in m_GuessBottonsRows[i_Row])
            {
                isSet &= guessButton.SetGuess;
            }

            return isSet;
        }

        public void ActivateRow(int i_RowNumber)
        {
            this.m_CurrentRound = i_RowNumber;
            for (int i = 0; i < k_NumberOfButtonsInGuess; i++)
            {
                m_GuessBottonsRows[i_RowNumber][i].Enabled = true;
            }

            m_PlayersGuesses[i_RowNumber] = new List<char>();
        }

        private void showScore(int i_Row, int i_CorrectInPlace, int i_CorrectMissPlace)
        {
            for (int bull = 0; bull < i_CorrectInPlace; bull++)
            {
                m_ScoreButtons[i_Row][bull].BackColor = Color.Black;
            }

            for (int p = 0; p < i_CorrectMissPlace && p + i_CorrectInPlace <= 4; p++)
            {
                m_ScoreButtons[i_Row][p + i_CorrectInPlace].BackColor = Color.Yellow;
            }
        }

        private void displaySequence()
        {
            Color[] buttonsColors = { Color.Pink, Color.Red, Color.LightGreen,
                                      Color.LightBlue, Color.Blue, Color.Yellow,
                                      Color.Brown, Color.White};
            string sequence = m_CurrentGame.GetWord();

        }

    }

    public class PickAColorForm : Form
    {
        private List<GuessButton> m_ColoredButtons = new List<GuessButton>();
        private List<char> m_Guess = new List<char>();
        private Color m_ChosenColor;
        private int m_Row;
        private int m_Colum;

        public Color ChosenColor
        {
            get
            {
                return m_ChosenColor;
            }
        }

        public List<GuessButton> ColoredButtons
        {
            get
            {
                return m_ColoredButtons;
            }
        }

        public List<char> Guess
        {
            get
            {
                return m_Guess;
            }
        }

        public PickAColorForm(int i_Row, int i_Colum, List<char> i_Guess) : base()
        {
            this.m_Guess = i_Guess;
            this.m_Row = i_Row;
            this.m_Colum = i_Colum;
            this.Size = new Size(190, 120);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Pick A Color";
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            initControls(m_Row, m_Colum);
            setControlsEventHandlers();
        }

        protected void initControls(int i_Row, int i_Colum)
        {
            Color[] buttonsColors = { Color.Magenta, Color.Red, Color.LightGreen,
                                      Color.LightBlue, Color.Blue, Color.Yellow,
                                      Color.Brown, Color.White};
            for (int i = 0; i < 8; i++)
            {
                char currentGuess = (char)('A' + i);
                GuessButton currentButton = new GuessButton(i_Row, i_Colum, currentGuess);
                currentButton.BackColor = buttonsColors[i];
                currentButton.Location = new Point(5 + ((i / 2) * 45),
                                                   5 + ((i % 2) * 45));
                if (m_Guess.Contains(currentGuess))
                {
                    currentButton.Enabled = false;
                }

                m_ColoredButtons.Add(currentButton);
            }

            this.Controls.AddRange(m_ColoredButtons.ToArray());
        }

        private void setControlsEventHandlers()
        {
            foreach (GuessButton colorButton in m_ColoredButtons)
            {
                colorButton.Click += new EventHandler(colorButton_Click);
            }
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            GuessButton colorButton = sender as GuessButton;
            if (colorButton != null)
            {
                m_Guess.Add(colorButton.Guess);
                m_ChosenColor = colorButton.BackColor;
            }
            this.Close();
        }


    }

    public class SequenceButton : Button
    {
        public SequenceButton() : base()
        {
            this.BackColor = Color.Black;
            this.Size = new Size(40, 40);
        }
    }

    public class GuessButton : SequenceButton
    {
        private int m_Colum;
        private int m_Row;
        private char m_Guess;
        private bool m_SetGuess = false;

        internal bool SetGuess
        {
            get
            {
                return m_SetGuess;
            }
            set
            {
                m_SetGuess = value;
            }
        }

        public char Guess
        {
            get
            {
                return m_Guess;
            }
            internal set
            {
                m_Guess = value;
            }
        }

        public int Colum
        {
            get
            {
                return m_Colum;
            }
        }

        public int Row
        {
            get
            {
                return m_Row;
            }
        }

        public GuessButton(int i_Row) : base()
        {
            //this.DialogResult = DialogResult.OK;
            this.m_Row = i_Row;
            this.BackColor = default(Color);

        }

        public GuessButton(int i_Row, int i_Colum) : this(i_Row)
        {
            this.m_Colum = i_Colum;
        }

        public GuessButton(int i_Row, int i_Colum, char i_Guess) : this(i_Row, i_Colum)
        {
            this.m_Guess = i_Guess;
        }
    }
}
