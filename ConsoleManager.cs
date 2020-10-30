using System;

namespace Hangman
{
	public static class ConsoleManager
	{
		/// Previous console colour for reset purposes
		public static readonly ConsoleColor PreviousBackground = Console.BackgroundColor;
		
		/// Previous console colour for reset purposes
		public static readonly ConsoleColor PreviousForeground = Console.ForegroundColor;
		
		public static void Write(string line) => Console.Write(line);
		public static void WriteLine(string line) => Console.WriteLine(line);
		
		public static void Clear()
		{
			Console.BackgroundColor = PreviousBackground;
			Console.ForegroundColor = PreviousForeground;
			Console.Clear();
		}
		public static void ClearNoBg() => Console.Clear();
		
		public static void SetBackground(ConsoleColor colour) => Console.BackgroundColor = colour;
		public static void SetText(ConsoleColor colour) => Console.ForegroundColor = colour;
	}
}