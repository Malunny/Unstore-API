namespace Unstore.DTOs;

public class ResultWarningMessage(string warningCode, string message)
{
    public string WarningCode {get; set;} = warningCode;
    public string Message { get; set; } = message;
}