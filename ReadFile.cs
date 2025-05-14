using Kit2;
namespace MyTest
{
	public class ReadFile : RunnableJob
	{
		public override void Run(object? args)
		{
			if (args is not Lexer l)
				throw new ArgumentException("args must be Lexer", nameof(args));

			if (!l.NextToken(eSkipMethods.SkipSpace))
				throw new Exception("Expecting file path.");

			var str = string.Empty;
			while (!l.IsCompleted())
			{
				str += l.token.value;
				if (!l.NextToken(eSkipMethods.None))
					continue;
			}

			if (string.IsNullOrEmpty(str))
				throw new Exception("Path cannot be empty.");

			var path = Path.GetFullPath(str);
			if (!File.Exists(path))
				throw new FileNotFoundException($"File not found: {path}");

			using (var reader = new StreamReader(path))
			{
				var content = reader.ReadToEnd();
				Log($"File content:\n{content}");
			}
		}
	}

}