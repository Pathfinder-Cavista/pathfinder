using PathFinder.API.Requests;
using PathFinder.Application.Commands.Samples;

namespace PathFinder.API.Mappers
{
    public static class SampleRequestMapper
    {
        public static CreateSampleCommand ToSampleCommand(CreateSampleRequest request)
        {
            return new CreateSampleCommand
            {
                Name = request.Name,
                Date = request.DueDate
            };
        }
    }
}
