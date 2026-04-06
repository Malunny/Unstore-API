using Unstore.Models;

namespace Unstore.Services;

public class ResultStatusMessage(OperationStatus statusCode, string errorMessage)
{
    public OperationStatus StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = errorMessage;
}

public static class ServiceResultExtensions
{
    public static bool IsBadResult<T>(this ServiceResult<T> serviceResult) where T : class
    {
        if (serviceResult.StatusMessage.Any(sm => (int)sm.StatusCode < 2000 && (int)sm.StatusCode >= 1000))
            return true;
        else 
            return false;
    }
}