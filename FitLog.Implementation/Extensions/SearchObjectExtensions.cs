﻿using AutoMapper;
using FitLog.Application.Exceptions;
using FitLog.Application.Localization;
using FitLog.Application.Search;
using FitLog.Application.Search.Attributes;
using FitLog.Application.Search.Enums;
using FitLog.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Extensions
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
            var searchObjProperties = search.GetType().GetProperties();

            foreach (var p in searchObjProperties)
            {
                var value = p.GetValue(search);

                if(value == null) continue;

                var attributes = p.GetCustomAttributes(true).Where(x => x is BaseSearchAttribute);

                foreach (BaseSearchAttribute attr in attributes)
                {
                    var comparisonType = attr.ComparisonType;

                    if (attr is QueryPropertyAttribute qp)
                    {
                        var properties = qp.Properties;

                        List<string> expressions = new List<string>();

                        foreach (var prop in properties)
                        {
                            expressions.Add(GetComparisonString(prop, value, comparisonType));
                        }

                        string separator = attr is IAndAttribute ? " && " : " || ";

                        query = query.Where("x => " + string.Join(separator, expressions));
                    }

                    if(attr is WithAnyPropertyAttribute wp)
                    {
                        var collection = wp.Collection;
                        var property = wp.Property;

                        query = query.Where("y => " + GetComparisonStringAnyProperty(collection, property, value, comparisonType));
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

                foreach(var arg in sortByArgs)
                {
                    var propAndDirection = arg.Split('.');

                    if(propAndDirection.Count() != 2)
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

                    if(!typeof(T).GetProperties().Any(x => x.Name.ToLower() == propAndDirection[0].ToLower()))
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
            switch(comparisonType)
            {
                case ComparisonType.Equals:
                    return $"x.{property} == {FormatValue(value)}";
                    break;
                case ComparisonType.Contains:
                    return $"x.{property}.Contains({FormatValue(value)})";
                    break;
                case ComparisonType.LessThan:
                    return $"x.{property} < {FormatValue(value)}";
                    break;
                case ComparisonType.LessThanOrEqual:
                    return $"x.{property} <= {FormatValue(value)}";
                    break;
                case ComparisonType.GreaterThan:
                    return $"x.{property} > {FormatValue(value)}";
                    break;
                case ComparisonType.GreaterThanOrEqual:
                    return $"x.{property} >= {FormatValue(value)}";
                    break;
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
            if (value is string) return $"\"{value}\"";

            return value;
        }
    }
}
