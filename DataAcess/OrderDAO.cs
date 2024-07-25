using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class OrderDAO
    {
        private static OrderDAO instance;
        public static OrderDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderDAO();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var _context = new ShopDbContext();
            var list = await _context.Orders.ToListAsync();
            return list;
        }

        public async Task<Order> GetOrderById(int? id)
        {
            var _context = new ShopDbContext();
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.OrderId == id);
            return order;
        }

        public async Task Add(Order order)
        {
            var _db = new ShopDbContext();
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Order order)
        {
            var _db = new ShopDbContext();
            var existingOrder = await GetOrderById(order.OrderId);
            if (existingOrder != null)
            {
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(Order order)
        {
            var _db = new ShopDbContext();
            var existingOrder = await GetOrderById(order.OrderId);
            if (existingOrder != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
        }
    }
}
