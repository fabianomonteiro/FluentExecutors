using System;
using System.Threading.Tasks;
using FluentExecutors.Derivations;
using FluentExecutors.Implementations.Models;

namespace FluentExecutors.Implementations.Commands
{
	public class AddPersonCommandAsync : BaseCommandResult<AddPersonCommandAsync, int>
	{
		public Person Person { get; set; }

		protected async override Task<int> ImplementExecuteAsync()
		{
			//Persist Database Add Person

			//Return Id from Added Person Persisted into Database

			throw new NotImplementedException();
		}
	}
}
