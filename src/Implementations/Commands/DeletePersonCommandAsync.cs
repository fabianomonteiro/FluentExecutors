using System;
using System.Threading.Tasks;
using FluentExecutors.Derivations;
using FluentExecutors.Implementations.Models;

namespace FluentExecutors.Implementations.Commands
{
	public class DeletePersonCommandAsync : BaseCommand<DeletePersonCommandAsync>
	{
		public Person Person { get; set; }

		protected async override Task ImplementExecuteAsync()
		{
			//Persist Database Update Person

			throw new NotImplementedException();
		}
	}
}
