﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.Logic
{
    public class Game
    {
		private List<char> m_WordToGuess;
        private const int k_NumberOfLettersInWord = 4;
        private const int k_MinNumberOfRounds = 4;
        private const int k_MaxNumberOfRounds = 10;
        private const char k_FirstLetterPossible = 'A';
        private const char k_LastLetterPossible = 'H';

		public int MinNumberOfGuesses
		{
			get
			{
				return k_MinNumberOfRounds;
			}
		}

		public int MaxNumberOfGuesses
		{
			get
			{
				return k_MaxNumberOfRounds;
			}
		}

		public void RandomizeNewWord()
		{
			List<char> wordToReturn = new List<char>();

			for (int i = 0; i < k_NumberOfLettersInWord; i++)
			{
				int nextLetterAsNumber = new Random().Next(k_FirstLetterPossible,
														   k_LastLetterPossible);
				if (!wordToReturn.Contains((char)nextLetterAsNumber))
				{
					char nextLetter = (char)nextLetterAsNumber;
					wordToReturn.Insert(i, nextLetter);
				}
				else
				{
					i--;
				}
			}

			m_WordToGuess = wordToReturn;
		}

		public string GetWord()
		{
			StringBuilder gamesWord = new StringBuilder();
			for (int i = 0; i < k_NumberOfLettersInWord; i++)
			{
				gamesWord.Append(m_WordToGuess[i]);
			}
			return gamesWord.ToString();
		}

		public void FeedbackForPlayerGuess(List<char> i_PlayersGuess, out int o_CorrectInPlace, out int o_CorrectMissPlace)
		{
			o_CorrectInPlace = 0;
			o_CorrectMissPlace = 0;
			List<char> feedbackOnGuess = new List<char>();

			for (int i = 0; i < k_NumberOfLettersInWord; i++)
			{
				// first cell is correctness of letter only, second cell is correctness of position
				List<bool> correctnessAndPosition = letterChecker(i_PlayersGuess[i], i);

				if (correctnessAndPosition[0] && !correctnessAndPosition[1])
				{
					o_CorrectMissPlace++;
				}

				if (correctnessAndPosition[0] && correctnessAndPosition[1])
				{
					o_CorrectInPlace++;
				}
			}

            

			//for (int i = 0; i < counterInPlace; i++)
			//{
			//	feedbackOnGuess.Add('V');
			//}

			//for (int i = 0; i < counterMissplaced; i++)
			//{
			//	feedbackOnGuess.Add('X');
			//}

			//for (int i = counterInPlace + counterMissplaced; i < k_NumberOfLettersInWord; i++)
			//{
			//	feedbackOnGuess.Add(' ');
			//}

			//return feedbackOnGuess;
		}

		private List<bool> letterChecker(char i_letterToCheck, int i_indexOfLetter)
		{
			List<bool> correctnessAndPosition = new List<bool>(2);
			correctnessAndPosition.Add(false);
			correctnessAndPosition.Add(false);
			bool letterNotFound = true;

			for (int i = 0; i < k_NumberOfLettersInWord && letterNotFound; i++)
			{
				if (i_letterToCheck == m_WordToGuess[i])
				{
					correctnessAndPosition[0] = true;
					if (i_indexOfLetter == i)
					{
						correctnessAndPosition[1] = true;
					}
					letterNotFound = false;
				}
			}

			return correctnessAndPosition;
		}
    }
}
