namespace Unstore.Services;

public readonly record struct DataServiceResult<T> : IServiceResult<T>
{
    public OperationStatus OperationStatus { get; } = OperationStatus.Ok;
    public T Data { get; }
    public bool Ok { get; }

    public DataServiceResult(OperationStatus operationStatus, bool ok, T data)
    {
        this.OperationStatus = operationStatus;
        this.Data = data;
        this.Ok = ok;
    }

    public DataServiceResult(bool ok, T data)
    {
        this.Data = data;
        this.Ok = true;
    }

    public DataServiceResult(OperationStatus operationStatus, bool ok)
    {
        this.OperationStatus = operationStatus;
        this.Ok = ok;
    }
}