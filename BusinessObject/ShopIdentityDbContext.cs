using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BusinessObject
{
    public class ShopIdentityDbContext : IdentityDbContext
    {
        public ShopIdentityDbContext(DbContextOptions<ShopIdentityDbContext> options)
            : base(options)
        { }

    }
}
