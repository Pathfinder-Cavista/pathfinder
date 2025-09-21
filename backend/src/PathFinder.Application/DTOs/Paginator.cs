namespace PathFinder.Application.DTOs
{
    public class Paginator<T>(List<T> items,
                     long count,
                     int pageNumber,
                     int pageSize)
    {
        public List<T> Items { get; set; } = items;
        public int CurrentPage { get; set; } = pageNumber;
        public int PageCount { get; set; } = (int)Math.Ceiling((double)count / pageSize);
        public long ItemCount { get; set; } = count;
        public bool HasPrevious
            => CurrentPage > PageCount;
        public bool HasNext
            => CurrentPage < PageCount;
    }
}
