namespace Unstore.DTOs;

public class ResultErrorMessage(ErrorCode errorCode, string errorMessage)
{
    public ErrorCode ErrorCode { get; set; } = errorCode;
    public string Message { get; set; } = errorMessage;
}