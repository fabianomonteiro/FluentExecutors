using FluentExecutors.Executors;

namespace FluentExecutors.Derivations
{
	public abstract class BaseUseCase<TImplementation> : Execution<TImplementation>
		where TImplementation : class, IExecution
	{
	}
}
