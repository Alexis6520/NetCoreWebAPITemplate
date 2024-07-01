namespace Services.Wrappers
{
    public class PaginatedList<T>
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}
