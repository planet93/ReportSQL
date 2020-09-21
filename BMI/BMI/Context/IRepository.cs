using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BMI.Context
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> GetAll();
        T Get(long id);
        T Get(Expression<Func<T, bool>> predicate);
        void Add(T item);
        void Edit(T item);
        void Delete(long id);
        void Save();
    }
}