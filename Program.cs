using System;
using System.Diagnostics;
namespace MyTest
{
	class Program
	{
		const StringComparison IGNORE = StringComparison.OrdinalIgnoreCase;
		static void Main()
		{
			using (var t = new Agent())
			{
				t.m_Token.Register(() => Console.WriteLine("End of program."));
				t.Run(null);
			}
		}
	}

}