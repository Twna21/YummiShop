using BusinessObject;
using Repository;
using DataAcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await OrderDAO.Instance.GetOrders();
        }

        public async Task<Order> GetOrderById(int? id)
        {
            return await OrderDAO.Instance.GetOrderById(id);
        }

        public async Task Add(Order order)
        {
            await OrderDAO.Instance.Add(order);
        }

        public async Task Update(Order order)
        {
            await OrderDAO.Instance.Update(order);
        }

        public async Task Delete(Order order)
        {
            await OrderDAO.Instance.Delete(order);
        }
    }
}
