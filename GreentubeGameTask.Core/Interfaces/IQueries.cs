using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GreentubeGameTask.Core.Interfaces
{
    public interface IQueries<T> where T : class
    {
        IQueryable<T> Query(List<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null);

        IQueryable<T> GetPage(out long count, int pageSize = 1, int skipRecords = 10,
            List<Expression<Func<T, bool>>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null);
    }
}