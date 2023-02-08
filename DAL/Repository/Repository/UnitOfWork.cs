using DAL.Data;
using DAL.Repository.IRepository;
using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;

        public IBaseRepo<Category> Categories { get; private set; }
        public IBaseRepo<Cover> Covers { get; private set; }

        public IBaseRepo<Product> Products { get; private set; }

        public IBaseRepo<ShoppingCart> ShoppingCarts { get; private set; }

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            Categories = new BaseRepo<Category>(_context);
            Covers = new BaseRepo<Cover>(_context);
            Products = new BaseRepo<Product>(_context);
            ShoppingCarts = new BaseRepo<ShoppingCart>(_context);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
