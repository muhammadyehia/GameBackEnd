using System.Collections.Generic;
using System.Data.Entity;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Infrastructure
{
    public class Commands<T> : ICommands<T> where T : class
    {
        private readonly DbContext _context;

        public Commands(DbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void AddBulk(IEnumerable<T> entities)
        {
            _context.BulkInsert(entities);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}