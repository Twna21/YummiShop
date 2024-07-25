using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

using DataAccess;

namespace Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public async Task<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            return await OrderDetailDAO.Instance.GetOrderDetails();
        }

        public async Task<OrderDetail> GetOrderDetailByIds(int orderId, int productId)
        {
            return await OrderDetailDAO.Instance.GetOrderDetailByIds(orderId, productId);
        }

        public async Task Add(OrderDetail orderDetail)
        {
            await OrderDetailDAO.Instance.Add(orderDetail);
        }

        public async Task Update(OrderDetail orderDetail)
        {
            await OrderDetailDAO.Instance.Update(orderDetail);
        }

        public async Task Delete(OrderDetail orderDetail)
        {
            await OrderDetailDAO.Instance.Delete(orderDetail);
        }
    }
}
