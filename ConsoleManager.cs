using System;

namespace Hangman
{
	public static class ConsoleManager
	{
		/// Previous console colour for reset purposes
		public static readonly ConsoleColor PreviousBackground = Console.BackgroundColor;
		
		/// Previous console colour for reset purposes
		public static readonly ConsoleColor PreviousForeground = Console.ForegroundColor;
	}
}