using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumDebugger;

namespace StartUp
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			
			DayOfWeek day = DayOfWeek.Friday;
			ConsoleModifiers keys = ConsoleModifiers.Alt | ConsoleModifiers.Shift;

			EnumDebugger.EnumDebugger.TestShowVisualizer(day);
			EnumDebugger.EnumDebugger.TestShowVisualizer(keys);
		}
	}
}
