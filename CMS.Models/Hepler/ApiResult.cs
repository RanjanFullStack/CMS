using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CMS.Models.Helper
{
    public class ApiResult<T>
    {
        private ApiResult(
            List<T> data,
            int count,
            int pageIndex,
            int pageSize,
            string sortColumn,
            string sortOrder,
            string filterColumn,
            string filterQuery)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            FilterColumn = filterColumn;
            FilterQuery = filterQuery;
        }

        // Async method for SQL Db (using Entity Framework)
        public static async Task<ApiResult<T>> CreateAsync(
            IQueryable<T> source,
            int pageIndex,
            int pageSize,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            // Apply filtering
            source = ApplyFilter(source, filterColumn, filterQuery);

            // Get total count after filtering
            var count = await source.CountAsync();

            // Apply sorting
            source = ApplySorting(source, sortColumn, sortOrder);

            // Apply pagination
            source = source.Skip(pageIndex * pageSize).Take(pageSize);

            // Get the data as a list asynchronously
            var data = await source.ToListAsync();

            return new ApiResult<T>(data, count, pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
        }

        // Synchronous method for JSON Db
        public static ApiResult<T> Create(
            IQueryable<T> source,
            int pageIndex,
            int pageSize,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            // Apply filtering
            source = ApplyFilter(source, filterColumn, filterQuery);

            // Get total count after filtering
            var count = source.Count();

            // Apply sorting
            source = ApplySorting(source, sortColumn, sortOrder);

            // Apply pagination
            source = source.Skip(pageIndex * pageSize).Take(pageSize);

            // Get the data synchronously
            var data = source.ToList();

            return new ApiResult<T>(data, count, pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
        }

        // Filter method (common for both async and sync)
        private static IQueryable<T> ApplyFilter(IQueryable<T> source, string filterColumn, string filterQuery)
        {
            if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterQuery) && IsValidProperty(filterColumn))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, filterColumn);
                var constant = Expression.Constant(filterQuery);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsExpression = Expression.Call(property, containsMethod, constant);
                var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
                source = source.Where(lambda);
            }

            return source;
        }

        // Sorting method (common for both async and sync)
        private static IQueryable<T> ApplySorting(IQueryable<T> source, string sortColumn, string sortOrder)
        {
            if (!string.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn))
            {
                sortOrder = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, sortColumn);
                var sortLambda = Expression.Lambda(property, parameter);

                var orderByMethod = sortOrder == "ASC" ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending);
                var orderByExpression = Expression.Call(
                    typeof(Queryable),
                    orderByMethod,
                    new Type[] { source.ElementType, property.Type },
                    source.Expression,
                    Expression.Quote(sortLambda));

                source = source.Provider.CreateQuery<T>(orderByExpression);
            }

            return source;
        }

        public static bool IsValidProperty(string propertyName, bool throwExceptionIfNotFound = true)
        {
            var prop = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            if (prop == null && throwExceptionIfNotFound)
                throw new NotSupportedException($"ERROR: Property '{propertyName}' does not exist.");
            return prop != null;
        }

        public List<T> Data { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => PageIndex > 0;
        public bool HasNextPage => (PageIndex + 1) < TotalPages;
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string FilterColumn { get; set; }
        public string FilterQuery { get; set; }
    }
}
