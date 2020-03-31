namespace FluentExecutors.Executors
{
	public static class ExecutorFactory
	{
		public static Executor<TExecution> Create<TExecution>() where TExecution : class, IExecution
		{
			return new Executor<TExecution>();
		}

		public static ExecutorResult<TExecution, TResult> Create<TExecution, TResult>() where TExecution : class, IExecutionResult<TResult>
		{
			return new ExecutorResult<TExecution, TResult>();
		}

		public static T Create<T>(T executor)
		{
			return executor;
		}
	}
}
