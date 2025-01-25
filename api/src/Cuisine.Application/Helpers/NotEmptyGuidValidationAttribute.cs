using System.ComponentModel.DataAnnotations;

namespace Cuisine.Application.Helpers;

public class NotEmptyGuidValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is Guid guid)
        {
            return guid != Guid.Empty;
        }
        return false;
    }
}