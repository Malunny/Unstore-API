namespace Unstore.Services;

public interface IServiceResultFactoryProvider
{
    IServiceResultFactory<T> Create<T>();
}