using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FluentExecutors.Executors
{
	public class ExecutorResult<TExecution, TResult> where TExecution : class, IExecutionResult<TResult>
	{
		private TExecution _execution;
		private TExecution Execution { get => _execution = _execution ?? Activator.CreateInstance<TExecution>(); }

		public TExecution GetExecution()
		{
			return Execution;
		}

		public bool CanExecute()
		{
			return Execution.CanExecute();
		}

		public TResult Execute()
		{
			return Execution.Execute();
		}

		public Task<TResult> ExecuteAsync(bool implementExecuteAsync = false)
		{
			return Execution.ExecuteAsync(implementExecuteAsync);
		}

		public ExecutorResult<TExecution, TResult> SetValue<TValue>(Expression<Func<TExecution, TValue>> destSelectorExpression, TValue value)
		{
			string propertyName = ExpressionHelper.GetBodyMemberName(destSelectorExpression);

			ReflectionHelper.SetValue(Execution, propertyName, value);

			return this;
		}
	}
}
