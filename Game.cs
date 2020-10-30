using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Hangman.ConsoleManager;

namespace Hangman
{
	public sealed class Game
	{
		/// Title of the console window.
		private const string Title = "Hangman";
		/// Dictionary file path.
		private const string DictionaryPath = "dictionary.txt";
		
		/// Dictionary to use for the instance.
		private static WordList dictionary;
		
		#region Static Instance Handling
		internal static Game instance { get; private set; }
		internal static void Main(string[] args)
		{
			// Setup
			Console.Title = Title;
			dictionary = new WordList(DictionaryPath);
			
			// Start game
			instance = new Game(dictionary);
			instance.Start();
			
			// Reset console
			SetBackground(PreviousBackground);
			SetText(PreviousForeground);
		}
		#endregion
		
		/// Dictionary instance
		private readonly WordList words;
		
		/// <summary>False while the instance is running.</summary>
		public bool finished { get; private set; } = false;
		
		// Constructor
		public Game(WordList dictionary) => words = dictionary;
		
		// Guesses & word
		public string word { get; private set; }
		private readonly List<char> guessesIncorrect = new List<char>();
		private readonly List<char> guessesCorrect = new List<char>();
		
		private const int totalGuesses = 9;
		private int guessesLeft => totalGuesses - guessesIncorrect.Count;
		
		// Game loop
		public void Start()
		{
			word = dictionary.PickWord().ToUpper();
			
			// Setup console
			SetBackground(ConsoleColor.Black);
			SetText(ConsoleColor.White);
			ClearNoBg();
			
			while(guessesLeft > 0)
			{
				PrintOverview();
				char guess = RequestGuess();
				
				if(guessesIncorrect.Contains(guess) || guessesCorrect.Contains(guess))
				{
					WriteLine($"You have already guessed \"{guess.ToString()}\".");
					continue;
				}
				
				if(!word.Contains(guess)) guessesIncorrect.Add(guess);
				else
				{
					guessesCorrect.Add(guess);
					if(CheckWord()) break;
				}
			}
			
			// Finished, print success/fail message
			WriteLine("");
			WriteLine(guessesLeft == 0 ? "You ran out of guesses." : "You win.");
			WriteLine($"The word was \"{word}\".");
			
			// Set status bool
			finished = true;
		}
		
		/**
		 * Prints the overview message that is shown before guessing.
		 */
		private void PrintOverview()
		{
			Write("Word:\t");			PrintWord();		Write("\n");
			Write("Guesses:\t");		PrintGuesses();		Write("\n");
		}
		
		/**
		 * Requests a valid character from the user.
		 */
		private static char RequestGuess()
		{
			string input;
			while(true)
			{
				Write("Enter guess: ");
				input = Console.ReadLine();
				
				// Error checking
				Debug.Assert(input != null, nameof(input) + " != null");
				if(input.Length == 1)	break;
				if(input.Length > 1)	Console.WriteLine($"\"{input}\" is not a guess as it contains more than 1 character.");
				if(input.Length == 0)	Console.WriteLine($"\"{input}\" is not a guess as it doesn't contain a character.");
			}
			return input.ToUpper().ToCharArray()[0];
		}
		
		/**
		 * Prints the current word,
		 * masking out unguessed characters.
		 */
		private void PrintWord()
		{
			ConsoleColor originalBg = Console.BackgroundColor;
			ConsoleColor originalTxt = Console.ForegroundColor;
			
			foreach(char c in word)
			{
				bool guessed = guessesCorrect.Contains(c);
				if(guessed)
				{
					SetBackground(ConsoleColor.White);
					SetText(ConsoleColor.Black);
					Write(c.ToString());
				}
				
				SetBackground(originalBg);
				SetText(originalTxt);
				
				if(!guessed) Write("_");
				
				Write(" ");
			}
		}
		
		/**
		 * Checks whether the current word is guessed.
		 */
		private bool CheckWord() => word.Aggregate(true, (current, character) => current & guessesCorrect.Contains(character));
		
		/**
		 * Print the current amount of guesses left.
		 */
		private void PrintGuesses()
		{
			ConsoleColor originalBg = Console.BackgroundColor;
			ConsoleColor originalTxt = Console.ForegroundColor;
			
			// Helper method
			void PrintCharList(IEnumerable<char> list, ConsoleColor bg, ConsoleColor txt)
			{
				foreach(char c in list)
				{
					// Set colour
					SetBackground(bg);
					SetText(txt);
					
					// Print character
					Write(c.ToString());
					
					// Set colour back
					SetBackground(originalBg);
					SetText(originalTxt);
					
					// Print space
					Write(" ");
				}
			}
			
			// Print good guesses
			PrintCharList(guessesCorrect, ConsoleColor.DarkGreen, ConsoleColor.White);
			
			// Print bad guesses
			PrintCharList(guessesIncorrect, ConsoleColor.Red, ConsoleColor.Black);
			
			// Print guesses left
			ConsoleColor bgColour;
			ConsoleColor txtColour;
			if(guessesLeft <= 1)
			{
				bgColour = ConsoleColor.Red;
				txtColour = ConsoleColor.Black;
			}
			else if(guessesLeft <= 3)
			{
				bgColour = ConsoleColor.Yellow;
				txtColour = ConsoleColor.Black;
			}
			else
			{
				bgColour = ConsoleColor.Green;
				txtColour = ConsoleColor.White;
			}
			SetBackground(bgColour);
			SetText(txtColour);
			
			Write
			(
				guessesLeft != 1
				?	$"({guessesLeft.ToString()} guesses left)"
				:	$"({guessesLeft.ToString()} guess left)"
			);
			
			SetBackground(originalBg);
			SetText(originalTxt);
		}
	}
}