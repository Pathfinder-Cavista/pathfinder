using FluentValidation;
using PathFinder.Application.Commands.Accounts;
using PathFinder.Application.Helpers;

namespace PathFinder.Application.Validations.Accounts
{
    public class TalentProfileUpdateCommandValidator : AbstractValidator<TalentProfileUpdateCommand>
    {
        public TalentProfileUpdateCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name field is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name field is required.");
            RuleFor(x => x.YearsOfExperience).GreaterThan(0).WithMessage("Years of experience must be greater than 0.");
            RuleFor(x => x.Location).NotEmpty().WithMessage("{PropertyName} field is required");
            RuleFor(x => x.ProfileSummary).NotEmpty().WithMessage("Profile Summary field is required");
            RuleFor(x => x)
                .Must(args => args.Skills.IsNotNullOrEmpty())
                .WithMessage("Please add one or more skills");
        }
    }
}
