using CommerceFlow.Services.Auth.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommerceFlow.Services.Auth.Application.Validators;

public sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty.")
            .Length(4,64).WithMessage("First name must be between 4-64 characters.");
        
        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .Length(4, 64).WithMessage("Last name must be between 4-64 characters.");
        
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Must be a valid e-mail address.")
            .Length(4, 256).WithMessage("E-mail must be between 4-256 characters.");
        
        RuleFor(u => u.PasswordHash)
            .NotNull().WithMessage("There is an error while hashing password.");
        
        RuleFor(u => u.PasswordSalt)
            .NotNull().WithMessage("There is an error while hashing password.");
    }
}