namespace Unstore.Services;

public class DataServiceResultFactory<T> : IServiceResultFactory<T>
{
    public IServiceResult<T> Success(T data)
        => new DataServiceResult<T>(true, data);
    public IServiceResult<T> Success(OperationStatus operationStatus, T data)
        => new DataServiceResult<T>(operationStatus, false, data);
    public IServiceResult<T> Failure(OperationStatus operationStatus)
        => new DataServiceResult<T>(operationStatus, false);
}