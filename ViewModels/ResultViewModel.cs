namespace Unstore.ViewModels;

public record ResultViewModel<T>
{
    public T? Data { get; }
    public List<string>? Errors { get; }
    public List<string>? Warnings { get; } 
    
    public ResultViewModel() { }

    public ResultViewModel(T data)
    {
        Data = data;
    }
    public ResultViewModel(T data, IEnumerable<string> errors)
    {
        Data = data;
        Errors = errors.ToList();
    }
    public ResultViewModel(T data, IEnumerable<string>? warnings = null, List<string>? errors = null)
    {
        Data = data;
        Errors = errors?.ToList() ?? null;
        Warnings = warnings?.ToList() ?? null;
    }
    public ResultViewModel(IEnumerable<string> errors, IEnumerable<string>? warnings = null)
    {
        Errors = errors.ToList();
        Warnings = warnings?.ToList() ?? null;
    }
    public ResultViewModel(IEnumerable<string> errors)
    {
        Errors = errors.ToList();
    }
}