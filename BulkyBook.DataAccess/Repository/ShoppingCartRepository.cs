using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }

        public int DecrementCount(ShoppingCart obj, int count)
        {
            obj.Count -= count;
            return obj.Count;
        }

        public int IncrementCount(ShoppingCart obj, int count)
        {
            obj.Count += count;
            return obj.Count;
        }

        public void Update(ShoppingCart obj)
        {
            
        }
    }
}
