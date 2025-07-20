

using Microsoft.EntityFrameworkCore;

namespace Hookpay.Shared.Core.Pagination;

public static class PaginationExtensions
{
    public class PagedResult<T>
    {
        public int CurrentPage { get; set; }    
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<T>? Results {  get; set; }
    }
    // ref: https://www.c-sharpcorner.com/article/pagination-in-c-sharp-complete-guide-with-easy-code-examples/
    public static async Task<PagedResult<T>> GetPagedData<T>(IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var result = new PagedResult<T>
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalCount = query.Count()
        };
        var pageCount = (int)Math.Ceiling(result.TotalCount / (double)pageSize);
        var skip = (pageNumber - 1) * pageSize;

        result.Results = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
        return result;
    }
}
    