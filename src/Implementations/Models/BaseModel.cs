namespace FluentExecutors.Implementations.Models
{
	public class BaseModel
	{
		public bool IsNew { get; private set; }

		public BaseModel() { }

		public void SetIsNew()
		{
			IsNew = true;
		}
	}
}
