using PathFinder.Application.Interfaces;
using PathFinder.Domain.Interfaces;

namespace PathFinder.Application.Features
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ISampleService> _sampleService;

        public ServiceManager(IRepositoryManager repository)
        {
            _sampleService = new Lazy<ISampleService>(() 
                => new SampleService(repository));
        }

        public ISampleService Sample => _sampleService.Value;
    }
}
