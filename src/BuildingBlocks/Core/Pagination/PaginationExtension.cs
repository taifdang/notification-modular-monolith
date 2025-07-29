

using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BuildingBlocks.Core.Pagination;

public static class PaginationExtension
{
    public class PageList<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
        public int TotalPage { get; set; }
        public IReadOnlyList<T>? Results { get; set; }
    }

    //ref: https://www.c-sharpcorner.com/article/pagination-in-c-sharp-complete-guide-with-easy-code-examples/
    public static async Task<PageList<T>> GetPageQuert<T>(
        IQueryable<T> query, 
        int pageNumber,
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var result = new PageList<T>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItem = query.Count()
        };

        result.TotalPage = (int)Math.Ceiling( result.TotalItem / (double) pageSize);

        var skip = (pageNumber - 1) * pageSize;

        result.Results = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

        return result;
    }
}
