using System;
using System.Threading.Tasks;

namespace FluentExecutors.Executors
{
	public abstract class Execution<TImplementation> : IDisposable, IExecution where TImplementation : class, IExecution
	{
		public object Result => throw new System.NotImplementedException();

		public virtual bool CanExecute()
		{
			return true;
		}

		public bool Execute()
		{
			if (!CanExecute())
				return false;

			BeforeExecuted();
			ImplementExecute();
			AfterExecuted();

			return true;
		}

		public virtual Task ExecuteAsync(bool implementExecuteAsync = false)
		{
			async Task execute()
			{
				if (!CanExecute())
					return;

				BeforeExecuted();

				if (implementExecuteAsync)
				{
					await ImplementExecuteAsync();
				}
				else
				{
					ImplementExecute();
				}

				AfterExecuted();
			}

			if (implementExecuteAsync)
				return execute();

			return Task.Run(async () => await execute());
		}

		protected virtual void BeforeExecuted() { }

		protected virtual void AfterExecuted() { }

		protected virtual void ImplementExecute() => throw new NotImplementedException();

		protected virtual Task ImplementExecuteAsync() => throw new NotImplementedException();

		public static Executor<TImplementation> GetExecutor()
		{
			return new Executor<TImplementation>();
		}

		public static Executor<TImplementation> GetExecutor(TImplementation implementationInstance)
		{
			return new Executor<TImplementation>(implementationInstance);
		}

		public virtual void Dispose() { }
	}
}
