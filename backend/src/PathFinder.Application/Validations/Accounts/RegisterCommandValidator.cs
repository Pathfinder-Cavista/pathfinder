using FluentValidation;
using PathFinder.Application.Commands.Accounts;
using PathFinder.Application.Helpers;
using System.Text.RegularExpressions;

namespace PathFinder.Application.Validations.Accounts
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("{PropertyName} must be later than the current date");
            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Enter a valid email address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
            RuleFor(x => x).Must(args => AccountHelpers.IsAValidPhoneNumber(args.PhoneNumber))
                .WithMessage("Please enter a valid phone number.");
            RuleFor(x => x.Role).IsInEnum().WithMessage("Please select a valid role");
        }
    }
}