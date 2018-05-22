using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Domain.Core;

namespace SmartValley.Data.SQL.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagingCollection<TEntity>> GetPageAsync<TEntity>(this IQueryable<TEntity> source, int offset, int count) where TEntity : class
        {
            var totalCount = await source.CountAsync();
            var entities = await source
                .Skip(offset)
                .Take(count)
                .ToArrayAsync();

            return new PagingCollection<TEntity>(entities, totalCount, offset);
        }
    }
}
