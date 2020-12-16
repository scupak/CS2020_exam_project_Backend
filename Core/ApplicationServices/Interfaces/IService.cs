using System.Collections.Generic;
using Core.Entities.Entities.Filter;

namespace Core.Services.ApplicationServices.Interfaces
{
    public interface IService<T, I>
    {
        public FilteredList<T> GetAll(Filter filter);
        public T GetById(I id);
        public T Add(T entity);
        public T Edit(T entity);
        public T Remove(I id);
    }
}
