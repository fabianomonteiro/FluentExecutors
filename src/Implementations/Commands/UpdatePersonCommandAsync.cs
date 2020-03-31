using System;
using System.Threading.Tasks;
using FluentExecutors.Derivations;
using FluentExecutors.Implementations.Models;

namespace FluentExecutors.Implementations.Commands
{
	public class UpdatePersonCommandAsync : BaseCommand<UpdatePersonCommandAsync>
	{
		public Person Person { get; set; }

		protected async override Task ImplementExecuteAsync()
		{
			//Persist Database Delete Person

			throw new NotImplementedException();
		}
	}
}
