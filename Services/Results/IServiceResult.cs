namespace Unstore.Services;

public interface IServiceResult<T>
{
    OperationStatus OperationStatus { get; }
    T Data { get; }
    bool Ok { get; }
}