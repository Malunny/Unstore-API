using Microsoft.AspNetCore.Mvc.ModelBinding;
using Unstore.DTOs;
using Unstore.Services;

namespace Unstore.Extensions;

public static class ModelStateGetErrors
{
    public static IEnumerable<ResultStatusMessage> GetErrors(this ModelStateDictionary modelState)
    {
        var errors = modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        
        List<ResultStatusMessage> resultErrors = new();
        
        foreach (var error in errors)
            resultErrors.Add(new ResultStatusMessage(OperationStatus.InvalidInput, error));
        
        return resultErrors;
    }
}