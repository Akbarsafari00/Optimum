using System.Text.Json.Serialization;

namespace Optimum.Persistence.Queries
{
    public class PagedResult<T> : PagedResultBase
    {
        public IEnumerable<T> Items { get; }

        public bool IsEmpty => Items is null || !Items.Any();
        public bool IsNotEmpty => !IsEmpty;

        protected PagedResult()
        {
            Items = Enumerable.Empty<T>();
        }

        [JsonConstructor]
        protected PagedResult(IEnumerable<T> items,
            int currentPage, int resultsPerPage,
            int totalPages, long totalResults) :
                base(currentPage, resultsPerPage, totalPages, totalResults)
        {
            Items = items;
        }

        public static PagedResult<T> Create(IEnumerable<T> items,
            int currentPage, int resultsPerPage,
            int totalPages, long totalResults)
            => new PagedResult<T>(items, currentPage, resultsPerPage, totalPages, totalResults);

        public static PagedResult<T> From(PagedResultBase result, IEnumerable<T> items)
            => new PagedResult<T>(items, result.CurrentPage, result.ResultsPerPage,
                result.TotalPages, result.TotalResults);

        public static PagedResult<T> Empty => new PagedResult<T>();

        public PagedResult<TU> Map<TU>(Func<T, TU> map)
            => PagedResult<TU>.From(this, Items.Select(map));
    }
}