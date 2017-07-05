using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Ex05.GUI
{
    public class BoardForm : Form
    {
        private readonly int r_NumberOfRounds;
        private const int k_NumberOfButtonsInGuess = 4;

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
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            initControls();
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

            for (int i = 0; i < r_NumberOfRounds; i++)
            {
                for (int j = 0; j < k_NumberOfButtonsInGuess; j++)
                {
                    Button grayButton = new GuessButton();
                    grayButton.Location = new Point(15 + (45 * j), 80 + (45 * i));
                    controls.Add(grayButton);
                }
                controls.Add(generateArrowButton(i));
				controls.AddRange(generateScoreButtons(i).ToArray());
            }


            this.Controls.AddRange(controls.ToArray());
        }

        private Button generateArrowButton(int i_RowNumber)
        {
            Button arrowButton = new Button();
            arrowButton.Size = new Size(40, 20);
            arrowButton.Location = new Point(195,90 + (45 * i_RowNumber));
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
                GuessButton scoreButton = new GuessButton();
                scoreButton.Size = new Size(15, 15);
                scoreButton.Location = new Point(this.ClientSize.Width - scoreButton.Width - 10 - (20 * (i % 2)),
                                                 80 + (20 * (i / 2) + (45 * i_RowNumber)));
                scoreButtons.Add(scoreButton);
            }

            return scoreButtons;
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
        public GuessButton() : base()
        {
            this.BackColor = Color.Gray;
        }
    }
}
