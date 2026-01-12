using Microsoft.EntityFrameworkCore;

namespace HRStpEmployee.Models
{
    public class PaginationResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int AllPages { get; set; }
        public int PageIndex { get; set; }
        public int TotalItems { get; set; }


        public PaginationResult(IEnumerable<T>items, int pageIndex,int count, int pageSize)
        {
            Items = items;
            PageIndex = pageIndex;
            AllPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;
        }

        public static async Task<PaginationResult<T>> CreatePages(IQueryable<T> items, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var count = await items.CountAsync();

            var resultItems = await items
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PaginationResult<T>(resultItems, pageIndex, count, pageSize);
        }

    }
}
