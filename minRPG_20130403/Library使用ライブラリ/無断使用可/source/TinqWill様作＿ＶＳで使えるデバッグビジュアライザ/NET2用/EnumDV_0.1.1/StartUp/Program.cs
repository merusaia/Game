using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain;

namespace StartUp
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			
			DayOfWeek day = DayOfWeek.Friday;
			ConsoleModifiers keys = ConsoleModifiers.Alt | ConsoleModifiers.Shift;

			PublicDomain.EnumDebugger.TestShowVisualizer(day);
			PublicDomain.EnumDebugger.TestShowVisualizer(keys);
		}
	}
}
