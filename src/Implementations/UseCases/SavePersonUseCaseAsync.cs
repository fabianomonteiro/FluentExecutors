using System.Threading.Tasks;
using FluentExecutors.Derivations;
using FluentExecutors.Executors;
using FluentExecutors.Implementations.Models;

namespace FluentExecutors.Implementations.UseCases
{
	public class SavePersonUseCaseAsync : BaseUseCase<SavePersonUseCaseAsync>
	{
		public Person Person { get; set; }

		protected async override Task ImplementExecuteAsync()
		{
			if (Person.IsNew)
			{
				this.Person.Id =
					await Fluent.Start(Commands.AddPersonCommandAsync.GetExecutor())
								.SetValue(x => x.Person, this.Person)
								.ExecuteAsync(true);
			}
			else
			{
				await Fluent.Start(Commands.UpdatePersonCommandAsync.GetExecutor())
							.SetValue(x => x.Person, this.Person)
							.ExecuteAsync(true);
			}
		}
	}
}
