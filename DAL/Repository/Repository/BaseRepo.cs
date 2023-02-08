using DAL.Data;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Repository
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly ApplicationDBContext _context;
        public BaseRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public void Add(T obj)
        {
            _context.Set<T>().Add(obj);
        }

        public void AddRange(IEnumerable<T> values)
        {
            _context.Set<T>().AddRange(values);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T Find(Expression<Func<T, bool>> expression)
        {
          return _context.Set<T>().FirstOrDefault(expression); 
        }

        public T Find(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Remove(T obj)
        {
            _context.Set<T>().Remove(obj);
        }

        public void RemoveRange(IEnumerable<T> values)
        {
            _context.Set<T>().RemoveRange(values);
        }

        public void Update(T obj)
        {
            _context.Set<T>().Update(obj);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).ToList();
        }
        public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return query.FirstOrDefault(criteria);
        }

       
    }
}
