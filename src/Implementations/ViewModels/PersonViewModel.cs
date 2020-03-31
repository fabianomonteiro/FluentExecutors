using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentExecutors.Executors;
using FluentExecutors.Implementations.Models;

namespace FluentExecutors.Implementations.ViewModels
{
	public class PersonViewModel
	{
		public IEnumerable<Person> Persons { get; private set; }

		public Person CurrentPerson { get; private set; }

		public string FilterText { get; set; }

		public PersonViewModel()
		{
			NewPerson();
		}

		public void NewPerson()
		{
			this.CurrentPerson = new Person();
			this.CurrentPerson.SetIsNew();
		}

		public void ClearPerson()
		{
			this.CurrentPerson.Id = null;
			this.CurrentPerson.Name = null;
		}

		public async Task ListPersonsAsync()
		{
			this.Persons =
				await Fluent.Start(Queries.ListPersonQueryAsync.GetExecutor())
							.SetValue(x => x.FilterText, this.FilterText)
							.ExecuteAsync(true);
		}

		public async Task SetCurrentPersonAsync(int id)
		{
			this.CurrentPerson = Persons.FirstOrDefault(x => x.Id == id);

			if (this.CurrentPerson == null)
			{
				var currentPerson = 
					await Fluent.Start(Queries.GetPersonByIdQueryAsync.GetExecutor())
								.SetValue(x => x.Id, id)
								.ExecuteAsync(true);

				if (currentPerson == null)
					throw new Exception("Person not found");

				this.CurrentPerson = currentPerson;
			}
		}

		public async Task SavePersonAsync()
		{
			await Fluent.Start(UseCases.SavePersonUseCaseAsync.GetExecutor())
						.SetValue(x => x.Person, this.CurrentPerson)
						.ExecuteAsync(true);
		}

		public async Task DeletePersonAsync()
		{
			if (CurrentPerson.IsNew)
				throw new Exception("A new Person cannot be deleted");

			await Fluent.Start(Commands.DeletePersonCommandAsync.GetExecutor())
						.SetValue(x => x.Person, this.CurrentPerson)
						.ExecuteAsync(true);
		}
	}
}
