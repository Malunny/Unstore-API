namespace Unstore.Services;

public class DataServiceResultFactoryProvider : IServiceResultFactoryProvider
{
    public IServiceResultFactory<T> Create<T>() =>
        new DataServiceResultFactory<T>();
}