using CommerceFlow.Services.Auth.Domain.Entities;
using FluentValidation;

namespace CommerceFlow.Services.Auth.Application.Validators;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(r => r.Name)
            .NotNull().WithMessage("Role name cannot be null.")
            .Length(4,64).WithMessage("Role name must be between 4-64 characters.");
    }
}