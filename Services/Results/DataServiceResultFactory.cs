namespace Unstore.Services;

public class DataServiceResultFactory : IServiceResultFactory
{
    public IServiceResult<T> Success<T>(T data)
        => new DataServiceResult<T>(ok: true, data);
    public IServiceResult<T> Success<T>(OperationStatus operationStatus, T data)
        => new DataServiceResult<T>(operationStatus, ok: true, data);
    public IServiceResult<T> Failure<T>(OperationStatus operationStatus)
        => new DataServiceResult<T>(operationStatus, ok: false);
}