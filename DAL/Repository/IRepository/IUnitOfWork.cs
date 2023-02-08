using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IBaseRepo<Category> Categories { get; }
        IBaseRepo<Cover> Covers { get; }
        IBaseRepo<Product> Products { get; }
        IBaseRepo<ShoppingCart> ShoppingCarts { get; }
        void Save();

    }
}
