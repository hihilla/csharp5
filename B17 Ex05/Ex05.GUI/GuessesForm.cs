using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05.GUI
{
    public class GuessesWindow : Form
    {
        Button m_ButtonNumberOfChances = new Button();
        Button m_ButtonStart = new Button();
        const int k_MaxSizeOfGuesses = 10;
        private int m_CounterOfGuessesClicks = 4;

        public int CounterOfGuessesClicks
        {
            get
            {
                return m_CounterOfGuessesClicks;
            }
        }

        public GuessesWindow()
        {
            this.Size = new System.Drawing.Size(150, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "BullsEye!";

            this.Controls.Add(m_ButtonNumberOfChances);
            this.Controls.Add(m_ButtonStart);

            this.m_ButtonNumberOfChances.Click += new EventHandler(NumberOfGuesses_Click);
        }

        private void InitGuesses()
        {
            m_ButtonStart.Text = "Start";
            m_ButtonStart.Location = new Point(this.ClientSize.Width - 8, 
                this.ClientSize.Height - m_ButtonStart.Height - 8);

            m_ButtonNumberOfChances.Text = string.Format("Number of chances: {0}", m_CounterOfGuessesClicks);
        }

        private void NumberOfGuesses_Click(Object sender, EventArgs e)
        {
            if (m_CounterOfGuessesClicks++ <= k_MaxSizeOfGuesses)
            {
                this.m_ButtonNumberOfChances.Text = string.Format("Number of chances {0}", m_CounterOfGuessesClicks);
            }
        }
    }
}
