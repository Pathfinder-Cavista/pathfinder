namespace PathFinder.Application.Interfaces
{
    public interface IServiceManager
    {
        IAccountService Account {  get; }
        IJobService Job { get; }
        IDataloadService Dataload { get; }
        IAnalyticsService Analytics { get; }
        IHolidayService Holiday { get; }
    }
}