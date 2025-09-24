using PathFinder.Application.DTOs;
using PathFinder.Application.Responses;

namespace PathFinder.Application.Interfaces
{
    public interface IHolidayService
    {
        Task<ApiBaseResponse> AddHolidayAsync(CreateHolidayCommand command);
        Task<ApiBaseResponse> DeleteHolidayAsync(Guid id);
        Task<ApiBaseResponse> GetHolidayList();
        Task<ApiBaseResponse> UpdateHolidayAsync(UpdateHolidayCommand command);
    }
}
