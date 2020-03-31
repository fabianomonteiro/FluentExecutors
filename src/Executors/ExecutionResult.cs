using System;
using System.Threading.Tasks;

namespace FluentExecutors.Executors
{
	public abstract class ExecutionResult<TExecutionImplementation, TResult> : IDisposable, IFluent<ExecutorResult<TExecutionImplementation, TResult>>, IExecutionResult<TResult> where TExecutionImplementation : class, IExecutionResult<TResult>
	{
		private bool _execute;

		private TResult _result;

		protected internal virtual bool IsAfterExecutedAsyncImplemented { get; internal set; }

		object IExecution.Result => _result;

		public virtual bool CanExecute()
		{
			return true;
		}

		public virtual TResult Execute()
		{
			_execute = false;

			if (!CanExecute())
				return default(TResult);

			_execute = true;

			_result = this.InternalExecute();

			return _result;
		}

		public virtual Task<TResult> ExecuteAsync(bool implementExecuteAsync = false)
		{
			async Task<TResult> execute()
			{
				_execute = false;

				if (!CanExecute())
					return default(TResult);

				_execute = true;

				_result = implementExecuteAsync ? await InternalExecuteAsync() : InternalExecute();

				return _result;
			}

			if (implementExecuteAsync)
				return execute();

			return Task.Run<TResult>(async () =>
			{
				return await execute();
			});
		}

		protected TResult InternalExecute()
		{
			return this.AfterExecuted(this.ImplementExecute());
		}

		protected async Task<TResult> InternalExecuteAsync()
		{
			if (!IsAfterExecutedAsyncImplemented)
				return this.AfterExecuted(await this.ImplementExecuteAsync());

			return await this.AfterExecutedAsync(this.ImplementExecuteAsync());
		}

		protected virtual TResult AfterExecuted(TResult result) => result;

		protected async virtual Task<TResult> AfterExecutedAsync(Task<TResult> result) => await result;

		protected virtual TResult ImplementExecute()
		{
			throw new NotImplementedException();
		}
		protected virtual Task<TResult> ImplementExecuteAsync()
		{
			throw new NotImplementedException();
		}

		bool IExecution.Execute()
		{
			Execute();

			return _execute;
		}

		Task IExecution.ExecuteAsync(bool implementExecuteAsync) => ExecuteAsync(implementExecuteAsync);

		public static ExecutorResult<TExecutionImplementation, TResult> GetExecutor()
		{
			return new ExecutorResult<TExecutionImplementation, TResult>();
		}

		ExecutorResult<TExecutionImplementation, TResult> IFluent<ExecutorResult<TExecutionImplementation, TResult>>.GetFluent()
		{
			return new ExecutorResult<TExecutionImplementation, TResult>();
		}

		object IFluent.GetFluent()
		{
			return ((IFluent<ExecutorResult<TExecutionImplementation, TResult>>)this).GetFluent();
		}

		public virtual void Dispose() { }
	}
}
