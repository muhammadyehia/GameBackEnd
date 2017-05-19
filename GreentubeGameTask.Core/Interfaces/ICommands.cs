using System.Collections.Generic;

namespace GreentubeGameTask.Core.Interfaces
{
    public interface ICommands<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void AddBulk(IEnumerable<T> entities);
    }
}