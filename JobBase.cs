namespace MyTest
{
	public abstract class RunnableJob : JobBase { }

	public abstract class JobBase : System.IDisposable
	{
		protected readonly CancellationTokenSource m_Source;
		public CancellationToken m_Token;
		private bool Isdisposed;

		public JobBase()
		{
			this.m_Source = new CancellationTokenSource();
			this.m_Token = m_Source.Token;
		}

		public abstract void Run(object? args);

		protected virtual void OnDispose(bool disposing) { }

		protected virtual void Dispose(bool disposing)
		{
			if (!Isdisposed)
			{
				if (disposing)
				{
					if (!m_Source.IsCancellationRequested)
						m_Source.Cancel();
				}
				m_Source.Dispose();

				m_Token = default;
				Isdisposed = true;
			}
		}

		~JobBase()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}


		protected void Log(string msg) => Console.WriteLine(msg);
	}
}