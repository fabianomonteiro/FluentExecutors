using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentExecutors.Derivations;
using FluentExecutors.Implementations.Models;

namespace FluentExecutors.Implementations.Queries
{
	public class ListPersonQueryAsync : BaseQuery<ListPersonQueryAsync, IEnumerable<Person>>
	{
		public string FilterText { get; set; }

		protected async override Task<IEnumerable<Person>> ImplementExecuteAsync()
		{
			// Run the query on the database to return the list of Person

			throw new NotImplementedException();
		}
	}
}
