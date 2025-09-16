using PathFinder.Application.Commands.Samples;
using PathFinder.Domain.Entities;

namespace PathFinder.Application.Mappers
{
    public static class SampleCommandsMapper
    {
        public static Sample ToEntity(CreateSampleCommand command)
        {
            return new Sample
            {
                Name = command.Name,
                Date = command.Date
            };
        }
    }
}
