using System.Threading.Tasks;

namespace FluentExecutors.Executors
{
	public interface IExecutionResult<T> : IExecution
	{
		new T Execute();

		new Task<T> ExecuteAsync(bool implementExecuteAsync = false);
	}
}
