using Microsoft.EntityFrameworkCore;
using PathFinder.Application.DTOs;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Responses;
using PathFinder.Application.Validations.Holiday;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;

namespace PathFinder.Application.Features
{
    public class HolidayService(IRepositoryManager repository) : IHolidayService
    {
        private readonly IRepositoryManager _repository = repository;

        public async Task<ApiBaseResponse> AddHolidayAsync(CreateHolidayCommand command)
        {
            var validator = new HolidayCommandsValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid input");
            }

            var holiday = new Holiday
            {
                Name = command.Name,
                Date = command.Date,
                IsRecurring = command.IsRecurring,
                Country = command.Country ?? "Global"
            };

            await _repository.Holiday.AddAsync(holiday);
            return new OkResponse<EntityIdDto>(new EntityIdDto(holiday.Id));
        }

        public async Task<ApiBaseResponse> UpdateHolidayAsync(UpdateHolidayCommand command)
        {
            var validator = new HolidayCommandsValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid input");
            }

            var holiday = await _repository.Holiday
                .GetOneAsync(h =>  h.Id == command.Id && !h.IsDeprecated);
            if(holiday == null)
            {
                return new NotFoundResponse("Holiday record not found");
            }

            holiday.Name = command.Name;
            holiday.Date = command.Date;
            holiday.Country = command.Country;
            holiday.IsRecurring = command.IsRecurring;
            holiday.ModifiedAt = DateTime.UtcNow;

            await _repository.Holiday.EditAsync(holiday);
            return new OkResponse<EntityIdDto>(new EntityIdDto(holiday.Id));
        }

        public async Task<ApiBaseResponse> DeleteHolidayAsync(Guid id)
        {
            var holiday = await _repository.Holiday
                .GetOneAsync(h => h.Id == id && !h.IsDeprecated);
            if (holiday == null)
            {
                return new NotFoundResponse("Holiday record not found");
            }

            holiday.IsDeprecated = true;
            holiday.ModifiedAt = DateTime.UtcNow;

            await _repository.Holiday.EditAsync(holiday);
            return new OkResponse<string>("Holiday record successfully deleted");
        }

        public async Task<ApiBaseResponse> GetHolidayList()
        {
            var holidays = (await _repository.Holiday
                .AsQueryable(h => !h.IsDeprecated)
                .ToListAsync())
                .Select(h => new HolidayDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Date = h.IsRecurring ?
                        new DateTime(DateTime.Today.Year, h.Date.Month, h.Date.Day) :
                        h.Date.Date,
                    IsRecurring = h.IsRecurring
                })
                .ToList();

            return new OkResponse<List<HolidayDto>>(holidays);
        }
    }
}
