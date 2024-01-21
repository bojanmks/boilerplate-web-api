using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Dynamic.Core;
using WebApi.Application.Exceptions;
using WebApi.Application.Localization;
using WebApi.Application.Search;
using WebApi.Application.Search.Attributes;
using WebApi.Application.Search.Enums;

namespace WebApi.Implementation.Extensions
{
    public static class SearchObjectExtensions
    {
        private static IMapper Mapper => ServiceProviderGetter.GetProvider().GetService<IMapper>();
        private static ITranslator Translator => ServiceProviderGetter.GetProvider().GetService<ITranslator>();

        public static object BuildDynamicQuery<T, TData>(this ISearchObject search, IQueryable<T> query)
        {
            query = search.BuildQuery(query);
            query = search.BuildOrderBy(query);

            var response = search.BuildResponse<T, TData>(query);

            return response;
        }

        public static object BuildResponse<T, TData>(this ISearchObject search, IQueryable<T> query)
        {
            if (search.Paginate)
            {
                return new PagedResponse<TData>
                {
                    Page = search.Page,
                    PerPage = search.PerPage,
                    TotalCount = query.Count(),
                    Items = Mapper.Map<IEnumerable<TData>>(query.Skip((search.Page - 1) * search.PerPage).Take(search.PerPage).ToList())
                };
            }

            return Mapper.Map<IEnumerable<TData>>(query.ToList());
        }

        public static IQueryable<T> BuildQuery<T>(this ISearchObject search, IQueryable<T> query)
        {
            var searchObjectProperties = search.GetType().GetProperties();

            foreach (var propertyInfo in searchObjectProperties)
            {
                var propertyValue = propertyInfo.GetValue(search);

                if (!propertyInfo.HasAttributeOfType<AllowNullAttribute>() && propertyValue is null)
                {
                    continue;
                }

                var searchAttributes = propertyInfo.GetAttributesOfType<BaseSearchAttribute>();

                foreach (BaseSearchAttribute attr in searchAttributes)
                {
                    var comparisonType = attr.ComparisonType;

                    if (attr is QueryPropertyAttribute qp)
                    {
                        var properties = qp.Properties;

                        List<string> expressions = new List<string>();

                        foreach (var prop in properties)
                        {
                            expressions.Add(GetComparisonString($"x.{prop}", propertyValue, comparisonType));
                        }

                        string separator = attr is IAndAttribute ? " && " : " || ";

                        query = query.Where("x => " + string.Join(separator, expressions));
                    }

                    if (attr is WithAnyPropertyAttribute wp)
                    {
                        var collection = wp.Collection;
                        var property = wp.Property;

                        query = query.Where("y => " + GetComparisonStringAnyProperty(collection, $"x.{property}", propertyValue, comparisonType));
                    }

                    if (attr is JoinedPropertiesAttribute jpa)
                    {
                        var propertyConcatanationString = jpa.BuildPropertyConcatanation();

                        var comparisonString = GetComparisonString(propertyConcatanationString, propertyValue, comparisonType);

                        query = query.Where("x => " + comparisonString);
                    }
                }
            }

            return query;
        }

        public static IEnumerable<TData> BuildQuery<T, TData>(this ISearchObject search, IQueryable<T> query)
        {
            query = search.BuildQuery(query);

            return Mapper.Map<IEnumerable<TData>>(query.ToList());
        }

        public static IQueryable<T> BuildOrderBy<T>(this ISearchObject search, IQueryable<T> query)
        {
            var sortByString = search.SortBy;

            if (!string.IsNullOrEmpty(sortByString))
            {
                var sortByArgs = sortByString.Split(',');
                string orderByClause = "";

                foreach (var arg in sortByArgs)
                {
                    var propAndDirection = arg.Split('.');

                    if (propAndDirection.Count() != 2)
                    {
                        throw new InvalidSortFormatException(Translator);
                    }

                    if (!SortDirections.Contains(propAndDirection[1]))
                    {
                        throw new InvalidSortDirectionException(Translator);
                    }

                    if (search.CustomSortBy.ContainsKey(propAndDirection[0]))
                    {
                        var customSortBy = search.CustomSortBy[propAndDirection[0]];

                        orderByClause += $"{customSortBy} {propAndDirection[1]},";
                        continue;
                    }

                    if (!typeof(T).GetProperties().Any(x => x.Name.ToLower() == propAndDirection[0].ToLower()))
                    {
                        throw new PropertyNotFoundException(Translator, propAndDirection[0]);
                    }

                    orderByClause += $"{propAndDirection[0]} {propAndDirection[1]},";
                }

                orderByClause = orderByClause.TrimEnd(',');

                query = query.OrderBy(orderByClause);
            }

            return query;
        }

        public static IEnumerable<TData> BuildOrderBy<T, TData>(this ISearchObject search, IQueryable<T> query)
        {
            query = search.BuildOrderBy(query);

            return Mapper.Map<IEnumerable<TData>>(query.ToList());
        }

        private static IEnumerable<string> SortDirections = new List<string> { "asc", "desc" };

        private static string GetComparisonString(string property, object value, ComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case ComparisonType.Equals:
                    return $"{property} == {FormatValue(value)}";
                case ComparisonType.Contains:
                    return $"{property}.Contains({FormatValue(value)})";
                case ComparisonType.LessThan:
                    return $"{property} < {FormatValue(value)}";
                case ComparisonType.LessThanOrEqual:
                    return $"{property} <= {FormatValue(value)}";
                case ComparisonType.GreaterThan:
                    return $"{property} > {FormatValue(value)}";
                case ComparisonType.GreaterThanOrEqual:
                    return $"{property} >= {FormatValue(value)}";
                default:
                    return $"x.{property} == {FormatValue(value)}";
            }
        }

        private static string GetComparisonStringAnyProperty(string collection, string property, object value, ComparisonType comparisonType)
        {
            return $"y.{collection}.Any(x => {GetComparisonString(property, value, comparisonType)})";
        }

        private static object FormatValue(object value)
        {
            if (value is null) return "null";

            if (value is string) return $"\"{value}\"";

            return value;
        }
    }
}
