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
            this.Size = new Size(150, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Bullseye";
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
                blackButton.Location = new Point(10 + (10 * i), 10);
                controls.Add(blackButton);
            }

            for (int i = 0; i < r_NumberOfRounds; i++)
            {
                for (int j = 0; j < k_NumberOfButtonsInGuess; j++)
                {
                    Button grayButton = new GuessButton();
                    grayButton.Location = new Point(10 + (10 * j), 30);
                    controls.Add(grayButton);
                }
                controls.Add(generateArrowButton());
            }

            controls.AddRange(generateScoreButtons().ToArray());

            this.Controls.AddRange(controls.ToArray());
        }

        private Button generateArrowButton()
        {
            Button arrowButton = new Button();
            arrowButton.Size = new Size(10, 5);
            arrowButton.Text = "-->";
            arrowButton.Enabled = false;
            return arrowButton;
        }

        private List<Button> generateScoreButtons()
        {
            List<Button> scoreButtons = new List<Button>();
            for (int i = 0; i < k_NumberOfButtonsInGuess; i++)
            {
                GuessButton scoreButton = new GuessButton();
                scoreButton.Size = new Size(5, 5);
                scoreButton.Location = new Point(this.ClientSize.Width - scoreButton.Width - 8 - (8 * (i % 2)),
                                                 this.ClientSize.Height - scoreButton.Height - 8 - (8 * (i % 2)));
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
            this.Size = new Size(10, 10);
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
