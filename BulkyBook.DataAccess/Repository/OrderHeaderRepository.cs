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

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
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
		public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
		{
			var obj = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
			if (obj != null)
			{
                obj.PaymentDate = DateTime.Now;
				obj.SessionId = sessionId;
				obj.PaymentIntentId = paymentIntentId;
			}
		}

	}
}
