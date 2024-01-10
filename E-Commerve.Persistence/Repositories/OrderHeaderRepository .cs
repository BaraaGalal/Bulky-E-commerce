using E_Commerce.Domain.Models;
using E_Commerce.Persistence.Data;
using E_Commerve.Persistence.Repositories.IRepositories;

namespace E_Commerve.Persistence.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                    orderFromDb.PaymentStatus = paymentStatus;
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId = null)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if (!string.IsNullOrEmpty(sessionId))
                orderFromDb.SessionId = sessionId;

            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                orderFromDb.SessionId = sessionId;
                orderFromDb.PaymentDate = DateTime.Now;
            }

        }
    }
}
