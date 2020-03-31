using FluentExecutors.Executors;

namespace FluentExecutors.Derivations
{
	public abstract class BaseCommand<TImplementation> : Execution<TImplementation>
		where TImplementation : class, IExecution
	{
	}
}
