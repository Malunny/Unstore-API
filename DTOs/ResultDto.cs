namespace Unstore.DTOs;

public class ResultDto<T>
{
    public T? Data { get; set; }
    public IEnumerable<ResultErrorMessage>? Errors { get; set; }
        = new List<ResultErrorMessage>();
    public IEnumerable<ResultWarningMessage>? Warnings { get; set; }
        = new List<ResultWarningMessage>();

    public ResultDto()
    {
        
    }
    public ResultDto(T data)
    {
        Data = data;
    }

    public ResultDto(T? data,
        IEnumerable<ResultErrorMessage>? errors = null,
        IEnumerable<ResultWarningMessage>? warnings = null)
    {
        Data = data;
        Errors = errors;
        Warnings = warnings;
    }
        public ResultDto(T? data,
        ResultErrorMessage? error = null,
        ResultWarningMessage? warning = null)
    {
        Data = data;
        Errors = new List<ResultErrorMessage> {error!};
        Warnings = new List<ResultWarningMessage> {warning!};
    }
}