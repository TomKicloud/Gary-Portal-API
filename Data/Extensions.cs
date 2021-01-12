using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GaryPortalAPI.Data
{
    public static class Extensions
    {
        public static IQueryable<TEntity> IncludeIf<TEntity>([NotNull] this IQueryable<TEntity> source, Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] navigationPropertyPaths)
        where TEntity : class
        {
            if (predicate(source.First()))
            {
                if (navigationPropertyPaths != null && navigationPropertyPaths.Length > 0)
                {
                    foreach (var navigationPropertyPath in navigationPropertyPaths)
                    {
                        source = source.Include(navigationPropertyPath);
                    }
                }
            }
                return source;
        }

        public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> transform)
        {
            return condition ? transform(source) : source;
        }

        public static IQueryable<T> If<T>(this IQueryable<T> source, Func<T, bool> predicate, Func<IQueryable<T>, IQueryable<T>> transform)
        {
            return predicate(source.First()) ? transform(source) : source;
        }
    }
}


