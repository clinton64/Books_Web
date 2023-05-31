using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository

    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }
        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus)
        {
            var obj = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (obj != null)
            {
                obj.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    obj.PaymentStatus = paymentStatus;
                }
            }
           
        }

    }
}
