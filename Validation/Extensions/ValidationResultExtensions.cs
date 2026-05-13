using CommerceFlow.Shared.Results;
using FluentValidation.Results;

namespace CommerceFlow.Shared.Validation.Extensions;

public static class ValidationResultExtensions
{
    public static List<string> ToErrorMessages(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .Select(error => error.ErrorMessage)
            .ToList();
    }

    public static ServiceResult ToServiceResult(this ValidationResult validationResult)
    {
        return ServiceResult.Fail(
            ResultStatus.ValidationError,
            "Validation error.",
            validationResult.ToErrorMessages()
        );
    }
}