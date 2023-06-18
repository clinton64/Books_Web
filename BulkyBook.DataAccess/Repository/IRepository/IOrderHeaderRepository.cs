using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatus(int id, string OrderStatus, string? paymentStatus = null);
        void UpdateStripePaymentId(int id,string sessionId , string stripePaymentId);
    }
}
