namespace Unstore.Services
{
    public enum OperationStatus
    {
        Ok = 1,
        Created = 2001,
        Updated = 2002,
        Deleted = 2003,
        UserAlreadyExists = 1001,
        InvalidCredentials = 1002,
        Unauthorized = 1003,
        NotFound = 1004,
        ValidationError = 1005,
        InternalServerError = 1006,
        InvalidInput = 1007,
    }

    public static class OperationStatusMethods
    {
        public static ResultStatusMessage ToResultStatusMessage(this OperationStatus operationStatus)
        {
            return operationStatus switch
            {
                OperationStatus.Ok => new ResultStatusMessage(operationStatus, "Ok"),
                OperationStatus.Created => new ResultStatusMessage(operationStatus, "Object Created"),
                OperationStatus.Deleted => new ResultStatusMessage(operationStatus, "Object Deleted"),
                OperationStatus.Updated => new ResultStatusMessage(operationStatus, "Object Updated"),
                OperationStatus.UserAlreadyExists => new ResultStatusMessage(operationStatus, "User already exists"),
                OperationStatus.InvalidCredentials => new ResultStatusMessage(operationStatus, "Invalid credentials"),
                OperationStatus.Unauthorized => new ResultStatusMessage(operationStatus, "Unauthorized access"),
                OperationStatus.NotFound => new ResultStatusMessage(operationStatus, "Resource not found"),
                OperationStatus.ValidationError => new ResultStatusMessage(operationStatus, "Validation error"),
                OperationStatus.InternalServerError => new ResultStatusMessage(operationStatus, "Internal server error"),
                OperationStatus.InvalidInput => new ResultStatusMessage(operationStatus, "Invalid Input"),
                _ => new ResultStatusMessage(operationStatus, "Unknown status")
            };
        }

    public static ResultStatusMessage InvalidInput(string customMessage)
        {
            return new ResultStatusMessage(OperationStatus.InvalidInput, customMessage);
        }
    }
}