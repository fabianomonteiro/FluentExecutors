using System;
using System.Threading.Tasks;
using FluentExecutors.Derivations;
using FluentExecutors.Implementations.Models;

namespace FluentExecutors.Implementations.Queries
{
	public class GetPersonByIdQueryAsync : BaseQuery<GetPersonByIdQueryAsync, Person>
	{
		public int Id { get; set; }

		protected async override Task<Person> ImplementExecuteAsync()
		{
			// Run the query on the database to return the Person

			throw new NotImplementedException();
		}
	}
}