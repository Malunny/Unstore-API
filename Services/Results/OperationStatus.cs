using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Unstore.Services
{
    public enum OperationStatus
    {
        Ok = 1,
        Created = 2001,
        Updated = 2002,
        Patched = 2003,
        Deleted = 2004,
        UserAlreadyExists = 1001,
        InvalidCredentials = 1002,
        InvalidLogin = 1003,
        Unauthorized = 1004,
        NotFound = 1005,
        ValidationError = 1006,
        InternalServerError = 1007,
        InvalidInput = 1008,
        ModelStateErrors = 1009
    }

    public static class OperationStatusToObjectResult
    {
        public static ObjectResult ToObjectResult<T>(this OperationStatus operationStatus, T obj)
            => operationStatus switch
            {
                OperationStatus.Ok => new ObjectResult(obj) { StatusCode = StatusCodes.Status200OK },
                OperationStatus.Created => new ObjectResult(obj) { StatusCode = StatusCodes.Status201Created },
                OperationStatus.Updated => new ObjectResult(obj) { StatusCode = StatusCodes.Status200OK },
                OperationStatus.Patched => new ObjectResult(obj) { StatusCode = StatusCodes.Status200OK },
                OperationStatus.Deleted => new ObjectResult(obj) { StatusCode = StatusCodes.Status204NoContent },
                OperationStatus.UserAlreadyExists => new ObjectResult(obj) { StatusCode = StatusCodes.Status409Conflict },
                OperationStatus.InvalidCredentials => new ObjectResult(obj) { StatusCode = StatusCodes.Status401Unauthorized },
                OperationStatus.InvalidLogin => new ObjectResult(obj) { StatusCode = StatusCodes.Status401Unauthorized },
                OperationStatus.Unauthorized => new ObjectResult(obj) { StatusCode = StatusCodes.Status401Unauthorized },
                OperationStatus.NotFound => new ObjectResult(obj) { StatusCode = StatusCodes.Status404NotFound },
                OperationStatus.ValidationError => new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest },
                OperationStatus.InternalServerError => new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError },
                OperationStatus.InvalidInput => new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest },
                OperationStatus.ModelStateErrors => new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest },
                _ => new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError }
            };

        public static bool IsBadResult(this OperationStatus status)
        {
            int value = (int)status;
            if (value >= 1000 & value <= 1999)
                return true;
            return false;
        }
    }
}