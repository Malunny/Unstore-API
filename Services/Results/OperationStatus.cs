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
}