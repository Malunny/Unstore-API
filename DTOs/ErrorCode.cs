namespace Unstore.DTOs
{
    public enum ErrorCode
    {
        UserAlreadyExists = 1001,
        InvalidCredentials = 1002,
        Unauthorized = 1003,
        NotFound = 1004,
        ValidationError = 1005,
        InternalServerError = 1006,
        InvalidInput = 1007
    }

    public static class ErrorCodeExtensions
    {
        public static ResultErrorMessage ToResultErrorMessage(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.UserAlreadyExists => new ResultErrorMessage(errorCode, "User already exists"),
                ErrorCode.InvalidCredentials => new ResultErrorMessage(errorCode, "Invalid credentials"),
                ErrorCode.Unauthorized => new ResultErrorMessage(errorCode, "Unauthorized access"),
                ErrorCode.NotFound => new ResultErrorMessage(errorCode, "Resource not found"),
                ErrorCode.ValidationError => new ResultErrorMessage(errorCode, "Validation error"),
                ErrorCode.InternalServerError => new ResultErrorMessage(errorCode, "Internal server error"),
                _ => new ResultErrorMessage(errorCode, "Unknown error")
            };
        }
    }
}