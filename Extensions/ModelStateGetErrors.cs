using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Unstore.DTOs;
using Unstore.Services;

namespace Unstore.Extensions;

public static class ModelStateGetErrors
{
    public static bool GetErrors(this ModelStateDictionary modelState)
    {
        var errors = modelState.Values.Any(x => x.Errors.Any());
        return errors;
    }
}