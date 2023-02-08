using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.IRepository
{
    public interface IBaseRepo<T> where T : class
    {
        T Find(int id);
        T Find(Expression<Func<T, bool>> expression);
        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        void Add(T obj);
        void Remove(T obj);
        void AddRange(IEnumerable<T> values);
        void RemoveRange(IEnumerable<T> values);
        void Update(T obj);
        
    }
}
