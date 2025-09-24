using FluentValidation;
using PathFinder.Application.DTOs;
using PathFinder.Application.Helpers;

namespace PathFinder.Application.Validations.Holiday
{
    public class HolidayCommandsValidator : AbstractValidator<BaseHolidayCommand>
    {
        public HolidayCommandsValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Holiday name is required");
            RuleFor(x => x)
                .Must(args => args.Date.IsValid()).WithMessage("Please enter a valid date");
        }
    }
}
