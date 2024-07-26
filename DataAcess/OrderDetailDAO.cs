using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance;
        private readonly ShopDbContext _context;

        private OrderDetailDAO()
        {
            _context = new ShopDbContext();
        }

        public static OrderDetailDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDetailDAO();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            return await _context.OrderDetails
                                 .Include(od => od.Product)
                                 .Include(od => od.Order)
                                 .ToListAsync();
        }

        public async Task<OrderDetail> GetOrderDetailByIds(int orderId, int productId)
        {
            return await _context.OrderDetails
                                 .Include(od => od.Product)
                                 .Include(od => od.Order)
                                 .SingleOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);
        }

        public async Task Add(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
