using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;
        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDAO();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<Category>> GetCategoryAll()
        {
            var _context = new ShopDbContext();
            var list = await _context.Categories.ToListAsync();
            return list;
        }

        public async Task<Category> GetCategoryById(int? id)
        {
            var _context = new ShopDbContext();
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == id);
            return category;
        }

        public async Task Add(Category category)
        {
            var _db = new ShopDbContext();
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Category category)
        {
            var _db = new ShopDbContext();
            var existingCategory = await GetCategoryById(category.CategoryId);
            if (existingCategory != null)
            {
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var _db = new ShopDbContext();
            var existingCategory = await GetCategoryById(id);
            if (existingCategory != null)
            {
                _db.Categories.Remove(existingCategory);
                await _db.SaveChangesAsync();
            }
        }
    }
}
