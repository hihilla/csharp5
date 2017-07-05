using System;
using System.Collections.Generic;

namespace Ex05.Logic
{
    public class Player
    {
		private bool m_QuiteGame = false;
		private int m_NumberOfRounds;
		private const int k_NumberOfLettersInWord = 4;
		private const int k_MinNumberOfRounds = 4;
		private const int k_MaxNumberOfRounds = 10;
		private const char k_FirstLetterPossible = 'A';
		private const char k_LastLetterPossible = 'H';
		private const char k_ExitLetter = 'Q';

		public int NumberOfLettersInWord
		{
			get { return k_NumberOfLettersInWord; }
		}

		public char FirstLetterPossible
		{
			get { return k_FirstLetterPossible; }
		}

		public char LastLetterPossible
		{
			get { return k_LastLetterPossible; }
		}

		public char ExitLetter
		{
			get { return k_ExitLetter; }
		}

		public int MinNumberOfRounds
		{
			get { return k_MinNumberOfRounds; }
		}

		public int MaxNumberOfRounds
		{
			get { return k_MaxNumberOfRounds; }
		}

		public bool QuiteGame
		{
			get
			{
				return m_QuiteGame;
			}
			set
			{
				m_QuiteGame = value;
			}
		}

		public int NumberOfRounds
		{
			get { return m_NumberOfRounds; }
			set
			{
				if (value >= k_MinNumberOfRounds && value <= k_MaxNumberOfRounds)
				{
					m_NumberOfRounds = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public bool IsNumOfRoundsInRange(int i_Round)
		{
			bool validGuess = true;
			try
			{
				this.NumberOfRounds = i_Round;
			}
			catch (ArgumentOutOfRangeException e)
			{
				validGuess = false;
			}
			return validGuess;
		}

        public List<char> GuessWord(string i_InputGuess)
		{
			List<char> userGuess = new List<char>();
			bool validGuess = false;

			while (!validGuess)
			{
				char currentInputLetter;
				int letterCounter = 0;
				bool validLetter = true;

				for (int i = 0; i < i_InputGuess.Length && validLetter; i += 2)
				{
					currentInputLetter = i_InputGuess[i];
					letterCounter++;
					if (currentInputLetter == k_ExitLetter)
					{
						m_QuiteGame = true;
						return null;
					}
					else if (userGuess.Contains(currentInputLetter))
					{
						throw new ArgumentException("Please use each letter only once");
					}
					else if (!InRange(currentInputLetter))
					{
						throw new ArgumentException(string.Format("Please use only letters {0} - {1}",
																  k_FirstLetterPossible,
																  k_LastLetterPossible));
					}
					else
					{
						userGuess.Add(currentInputLetter);
					}
				}

				if (letterCounter != NumberOfLettersInWord || !validLetter)
				{
					validGuess = false;
					userGuess.Clear();
				}
				else
				{
					validGuess = true;
				}
			}

			return userGuess;
		}

		public bool InRange(char i_Letter)
		{
			bool letterInRange = (i_Letter >= k_FirstLetterPossible) && (i_Letter <= k_LastLetterPossible);

			return letterInRange;
		}
    }
}
