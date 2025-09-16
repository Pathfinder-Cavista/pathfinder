using PathFinder.Application.Commands.Samples;
using PathFinder.Application.DTOs;

namespace PathFinder.Application.Interfaces
{
    public interface ISampleService
    {
        Task<SampleDto> AddSample(CreateSampleCommand command);
    }
}