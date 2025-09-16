using FluentValidation;
using PathFinder.Application.Commands.Samples;

namespace PathFinder.Application.Validations.Sample
{
    internal class CreateSampleValidator : AbstractValidator<CreateSampleCommand>
    {
        public CreateSampleValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("{PropertyName} is required.");
            RuleFor(x => x.Date.Date).GreaterThan(DateTime.UtcNow.Date)
                .WithMessage("{PropertyName} must be later than the current date");
        }
    }
}
