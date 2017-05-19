using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Infrastructure
{
    public class Queries<T> : IQueries<T> where T : class
    {
        private readonly DbContext _context;

        public Queries(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetPage(out long count, int pageSize = 1, int skipRecords = 10,
            List<Expression<Func<T, bool>>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null)
        {
            var items = Query(filters, orderBy, includes);
            count = items.Count();
            return count > 0 ? items.Skip(skipRecords).Take(pageSize) : null;
        }

        public IQueryable<T> Query(List<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            query = query.Where(p => true);
            if (filters != null)
            {
                query = filters.Aggregate(query, (current, item) => current.Where(item));
                if (includes != null)
                    query = includes.Aggregate(query, (current, item) => current.Include(item));
            }

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }
    }
}