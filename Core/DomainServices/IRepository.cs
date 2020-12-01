using System.Collections.Generic;

namespace Core.Services.DomainServices
{
    public interface IRepository<T>
    {
        public List<T> GetAll();
        public T Get(int id);
        public T Add(T entity);
        public T Edit(T entity);
        public T Remove(int id);
        public int Count();

    }
}
