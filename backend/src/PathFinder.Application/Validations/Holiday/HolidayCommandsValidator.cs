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
                .Must(args => ValidHolidayDate(args.Date, args.IsRecurring)).WithMessage("Please enter a valid date");
        }

        private bool ValidHolidayDate(DateTime date, bool recurring)
        {
            return !recurring ? date.Date >= DateTime.UtcNow.Date && date.IsValid() :
                date.IsValid();
        }
    }
}
