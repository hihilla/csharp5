﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05.Logic
{
    public class GuessesManager : Form
    {
        Button m_ButtonNumberOfChances = new Button();
        Button m_ButtonStart = new Button();
        const int k_MaxSizeOfGuesses = 10;
        private int m_CounterOfGuessesClicks = 4;

        public GuessesManager()
        {
            this.Size = new System.Drawing.Size(150, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "BullEye!";

            this.Controls.Add(m_ButtonNumberOfChances);
            this.Controls.Add(m_ButtonStart);

            this.m_ButtonNumberOfChances.Click += new EventHandler(NumberOfGuesses_Click);

        }

        private void InitGuesses()
        {
            m_ButtonStart.Text = "Start";
            m_ButtonStart.Location = new Point(this.ClientSize.Width - 8, 
                this.ClientSize.Height - m_ButtonStart.Height - 8);

            m_ButtonNumberOfChances.Text = "Number of chances: 4";
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
