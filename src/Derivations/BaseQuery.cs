using FluentExecutors.Executors;

namespace FluentExecutors.Derivations
{
	public abstract class BaseQuery<TImplementation, TResult> : ExecutionResult<TImplementation, TResult>
		where TImplementation : class, IExecutionResult<TResult>
	{
	}
}
