using System;
using System.IO;

namespace Hangman
{
	public class WordList
	{
		/// Random instance
		protected readonly Random random;
		
		/// Word array to pick from
		public readonly string[] words;
		
		// Constructor
		public WordList(string path, string separator = "\n")
		{
			// Check file exists
			if(!File.Exists(path))
				throw new FileNotFoundException($"Could not find the specified word list dictionary \"{path}\"");
			
			random = new Random();								// Random instance
			words = File.ReadAllText(path).Split(separator);	// Read file contents
		}
		
		/**
		 * <summary>Picks a random word from the dictionary.</summary>
		 * <returns>The random word picked from the dictionary</returns>
		 */
		public string PickWord() => words[random.Next(words.Length)];
	}
}