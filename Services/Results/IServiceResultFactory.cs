namespace Unstore.Services;

public interface IServiceResultFactory<T>
{
    public IServiceResult<T> Success(T data);
    public IServiceResult<T> Failure(OperationStatus status);
    public IServiceResult<T> Success(OperationStatus status, T data);
    
}