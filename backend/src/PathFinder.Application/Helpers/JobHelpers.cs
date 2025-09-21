namespace PathFinder.Application.Helpers
{
    public class JobHelpers
    {
        public static bool IsAValidClosingDate(DateTime? date)
        {
            return !date.HasValue || (date.HasValue && date.Value.Date > DateTime.UtcNow.Date);
        }
    }
}
