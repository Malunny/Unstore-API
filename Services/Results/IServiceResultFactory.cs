namespace Unstore.Services;

public interface IServiceResultFactory
{
    public IServiceResult<T> Success<T>(T data);
    public IServiceResult<T> Failure<T>(OperationStatus status);
    public IServiceResult<T> Success<T>(OperationStatus status, T data);
    
}