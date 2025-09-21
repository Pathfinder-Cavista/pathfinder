using FluentValidation;
using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.Helpers;

namespace PathFinder.Application.Validations.Jobs
{
    public class JobCommandsValidator : AbstractValidator<JobWriteCommand>
    {
        public JobCommandsValidator()
        {
            RuleFor(x => x.JobTitle).NotEmpty()
                .WithMessage("Job Title field is required.");
            RuleFor(x => x.Description).NotEmpty()
               .WithMessage("Job Description field is required."); 
            RuleFor(x => x.EmploymentType).IsInEnum()
                .WithMessage("Please enter a valid employment type.");
            RuleFor(x => x)
                .Must(args => JobHelpers.IsAValidClosingDate(args.DeadLine))
                .WithMessage("The closing date must be later than the current date");
            RuleFor(x => x.Level).IsInEnum()
                .WithMessage("Please enter a valid employment level.");

        }
    }
}