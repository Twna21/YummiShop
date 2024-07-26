using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrderById(int? id);
        Task Add(Order order);
        Task Update(Order order);
        Task Delete(Order order);
    }
}
