using System.Reflection;
using Kit2;
namespace MyTest
{
	public class ExecuteRunnable : JobBase
	{
		public override void Run(object? args)
		{
			if (args is not Lexer l)
				throw new ArgumentException("args must be Lexer", nameof(args));

			while (!l.IsCompleted())
			{
				if (!l.NextToken(eSkipMethods.SkipAll))
					continue;
				if (!l.token.IsIdentifier())
					throw new Exception($"Expecting runnable name, but got {l.token.value}.");

				var runnableName = l.token.value;
				var assembly = Assembly.GetExecutingAssembly();
				var type = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(RunnableJob))).FirstOrDefault();
				if (type == null)
					throw new Exception($"Runnable not found: {runnableName}");

				// Create instance of the runnable job
				try
				{
					using (var job = Activator.CreateInstance(type) as RunnableJob)
					{
						if (job == null)
							throw new Exception($"Failed to create instance of {runnableName}");
						Log($"> {type.Name}");
						job.Run(l);
					}
				}
				catch (Exception ex)
				{
					Log($"Error running {runnableName}: {ex.Message}");
				}
				finally
				{

				}
				return;
			}
		}
	}

}