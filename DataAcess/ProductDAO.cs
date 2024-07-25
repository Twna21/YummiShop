using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess
{
    public class ProductDAO
    {
        private static ProductDAO instance;
        public static ProductDAO Instance
        {

            get
            {
                if (instance == null)
                {
                    instance = new ProductDAO();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var _context = new ShopDbContext();
            var list = await _context.Products.ToListAsync();

            return list;
        }

        public async Task<Product> GetProductById(int? id)
        {
            var _context = new ShopDbContext();
            var Product = await _context.Products.SingleOrDefaultAsync(s => s.ProductId == id);
            return Product;
        }

       


        public async Task Add(Product pro)
        {
            var _db = new ShopDbContext();
            _db.Products.Add(pro);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Product pro)
        {
            var _db = new ShopDbContext();
            var o = await GetProductById(pro.ProductId);
            if (o != null)
            {
                _db.Products.Update(pro);
                await _db.SaveChangesAsync();
            }

        }

        public async Task Delete(Product pro)
        {
            var _db = new ShopDbContext();
            var o = await GetProductById(pro.ProductId);
            if (o != null)
            {
                _db.Products.Remove(pro);
                await _db.SaveChangesAsync();
            }

        }

    }
}
