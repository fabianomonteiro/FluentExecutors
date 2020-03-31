namespace FluentExecutors.Executors
{
	public interface IFluent
	{
		object GetFluent();
	}

	public interface IFluent<TFluent> : IFluent
	{
		new TFluent GetFluent();
	}
}
