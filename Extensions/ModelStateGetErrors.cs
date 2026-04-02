using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Unstore.Extensions;

public static class ModelStateGetErrors
{
    public static IEnumerable<string> GetErrors(this ModelStateDictionary modelState)
    {
        return modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
    }
}