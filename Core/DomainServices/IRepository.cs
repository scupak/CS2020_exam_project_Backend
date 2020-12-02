using System.Collections.Generic;

namespace Core.Services.DomainServices
{
    public interface IRepository<T, I>
    {
        public List<T> GetAll();
        public T GetById(I id);
        public T Add(T entity);
        public T Edit(T entity);
        public T Remove(I id);
        public int Count();

    }
}
