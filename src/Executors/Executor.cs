using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FluentExecutors.Executors
{
	public class Executor<TExecution> where TExecution : class, IExecution
	{
		private TExecution _execution;
		private TExecution Execution { get => _execution = _execution ?? Activator.CreateInstance<TExecution>(); }

		public Executor() { }

		public Executor(TExecution execution)
		{
			_execution = execution;
		}

		public Executor<TExecution> Execute()
		{
			Execution.Execute();
			return this;
		}

		public Task ExecuteAsync(bool implementExecuteAsync = false)
		{
			return Execution.ExecuteAsync(implementExecuteAsync);
		}

		public Executor<TExecution> SetValue<TValue>(Expression<Func<TExecution, TValue>> destSelectorExpression, TValue value)
		{
			string propertyName = ExpressionHelper.GetBodyMemberName(destSelectorExpression);

			ReflectionHelper.SetValue(Execution, propertyName, value);

			return this;
		}
	}
}
