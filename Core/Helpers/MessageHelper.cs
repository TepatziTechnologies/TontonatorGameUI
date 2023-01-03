using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tontonator.Core.Helpers
{
	internal class MessageHelper
	{
		public static void WriteError(string message)
		{
			Console.BackgroundColor = ConsoleColor.DarkRed;
			Console.WriteLine(message);
			Console.BackgroundColor = ConsoleColor.Black;
		}

		public static void WriteSuccess(string message)
        {
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine(message);
			Console.BackgroundColor = ConsoleColor.Black;
        }
	}
}
