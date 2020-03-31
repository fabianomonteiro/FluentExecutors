using System.Threading.Tasks;

namespace FluentExecutors.Executors
{
	public interface IExecution
	{
		object Result { get; }

		bool CanExecute();

		bool Execute();

		Task ExecuteAsync(bool implementExecuteAsync = false);
	}
}
