namespace WebApi.Implementation.Extensions
{
    public static class QueryableExtensions
    {
        public static bool IsOrdered<T>(this IQueryable<T> query)
        {
            return query.Expression.Type == typeof(IOrderedQueryable<T>);
        }
    }
}
