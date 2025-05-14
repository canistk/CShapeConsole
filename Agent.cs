using Kit2;
namespace MyTest
{
	public class Agent : JobBase
	{
		public override void Run(object? args)
		{
			// Wait for user input and define execute action
			while (!m_Token.IsCancellationRequested)
			{
				try
				{
					if (!TryGetUserInput(out var input))
						continue;
					AnalysisCmd(input);
				}
				catch (AggregateException ex)
				{
					foreach (var inner in ex.InnerExceptions)
					{
						Log(inner.Message);
					}
				}
				catch (LexerException ex)
				{
					Log("LexerException : " + ex.Message);
				}
				catch (Exception ex)
				{
					Log("NonHandle Exception : " + ex.Message);
				}
			}
			Log("End of test.");
		}

		private void AnalysisCmd(string rawString)
		{
			var l = new Lexer(rawString);
			while (!l.IsCompleted())
			{
				if (!l.NextToken(eSkipMethods.SkipAll))
					continue;

				if (l.token.IsIdentifier())
				{
					var cmd = l.token.value.ToLower();
					switch (cmd)
					{
						case "exit":
						case "quit":
						case "stop":
						{
							m_Source.Cancel();
							return;
						}

						case "cls":
						case "clear":
						{
							Console.Clear();
						}
						break;

						case "list":
						{
							if (!l.NextToken(eSkipMethods.SkipAll))
								throw new LexerException(l, "Expecting runnable name after 'list' command.");
							if (l.token.IsIdentifier("runnable", true))
							{
								using (var j = new ListRunnable())
								{
									j.m_Token.Register(() => Log("End of command.\n\n"));
									j.Run(l);
								}
								return;
							}
						}
						break;

						default:
						{
							if (l.token.IsIdentifier())
							{
								// Try execute
								using (var j = new ExecuteRunnable())
								{
									j.m_Token.Register(() => Log("End of command.\n\n"));
									j.Run(l);
								}
								return;
							}
							else
							{
								Log($"Unknown command: {cmd}.");
							}
						}
						break;
					}
				}
			}
		}

		private bool TryGetUserInput(out string input)
		{
			input = string.Empty;
			while (string.IsNullOrEmpty(input))
			{
				Log("Wait for user input:");
				input = Console.ReadLine();
				if (string.IsNullOrEmpty(input))
					continue;
			}
			return true;
		}
	}

}