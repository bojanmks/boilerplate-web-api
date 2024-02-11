namespace WebApi.Application.Search
{
    public interface ISearchObjectQueryBuilder
    {
        object BuildDynamicQuery<T, TData>(ISearchObject search, IQueryable<T> query);
        object BuildResponse<T, TData>(ISearchObject search, IQueryable<T> query);
        IQueryable<T> BuildQuery<T>(ISearchObject search, IQueryable<T> query);
        IEnumerable<TData> BuildQuery<T, TData>(ISearchObject search, IQueryable<T> query);
        IQueryable<T> BuildOrderBy<T>(ISearchObject search, IQueryable<T> query);
        IEnumerable<TData> BuildOrderBy<T, TData>(ISearchObject search, IQueryable<T> query);
    }
}
