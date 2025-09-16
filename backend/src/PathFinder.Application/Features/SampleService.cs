using PathFinder.Application.Commands.Samples;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Mappers;
using PathFinder.Application.Validations.Sample;
using PathFinder.Domain.Interfaces;

namespace PathFinder.Application.Features
{
    public sealed class SampleService : ISampleService
    {
        private readonly IRepositoryManager _repository;

        public SampleService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<SampleDto> AddSample(CreateSampleCommand command)
        {
            var validator = new CreateSampleValidator().Validate(command);
            if (!validator.IsValid)
            {
                throw new BadRequestException(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid request");
            }

            var sample = SampleCommandsMapper.ToEntity(command);
            await _repository.Sample.AddAsync(sample);

            await _repository.SaveAsync();
            return SampleDto.FromEntity(sample);
        }
    }
}