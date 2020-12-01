using System.Collections.Generic;

namespace Core.Services.ApplicationServices.Interfaces
{
    public interface IService<T, I>
    {
        public List<T> GetAll();
        public T Get(I id);
        public T Add(T entity);
        public T Edit(T entity);
        public T Remove(I id);
    }
}
