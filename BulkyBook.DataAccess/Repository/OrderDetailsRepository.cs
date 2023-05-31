using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetail>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }

        public void Update(OrderDetail obj)
        {
           _context.OrderDetails.Update(obj);
        }

    }
}
