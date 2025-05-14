using System.Reflection;
using Kit2;
namespace MyTest
{
	public class ListRunnable : JobBase
	{
		public override void Run(object? args)
		{
			if (args is not Lexer lexer)
				throw new ArgumentException("args must be Lexer", nameof(args));

			var assembly = Assembly.GetExecutingAssembly();
			var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(RunnableJob))).ToArray();

			var cnt = types.Length;
			Log($"Runnable List: {cnt}");
			for (int i = 0; i < cnt; ++i)
			{
				if (!types[i].IsSubclassOf(typeof(RunnableJob)))
					continue;
				Log($"[{i:00}] {types[i].Name}");
			}
		}
	}

}