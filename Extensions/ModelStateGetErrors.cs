using Microsoft.AspNetCore.Mvc.ModelBinding;
using Unstore.DTOs;

namespace Unstore.Extensions;

public static class ModelStateGetErrors
{
    public static IEnumerable<ResultErrorMessage> GetErrors(this ModelStateDictionary modelState)
    {
        var errors = modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        
        List<ResultErrorMessage> resultErrors = new();
        
        foreach (var error in errors)
        {
            resultErrors.Add(new ResultErrorMessage(ErrorCode.InvalidInput, error));
        }
        
        return resultErrors;
    }
}