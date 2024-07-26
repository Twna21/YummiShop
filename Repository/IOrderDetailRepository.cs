using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetails();
        Task<OrderDetail> GetOrderDetailByIds(int orderId, int productId);
        Task Add(OrderDetail orderDetail);
        Task Update(OrderDetail orderDetail);
        Task Delete(OrderDetail orderDetail);
    }
}
