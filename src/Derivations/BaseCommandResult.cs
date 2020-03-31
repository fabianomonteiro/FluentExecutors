using FluentExecutors.Executors;

namespace FluentExecutors.Derivations
{
	public abstract class BaseCommandResult<TImplementation, TResult> : ExecutionResult<TImplementation, TResult>
		where TImplementation : class, IExecutionResult<TResult>
	{
	}
}
