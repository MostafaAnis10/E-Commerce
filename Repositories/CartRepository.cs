using E_Commerce.DataAccess;
using E_Commerce.Models;
using E_Commerce.Repositories.IRepositories;

namespace E_Commerce.Repositories
{
    public class CartRepository :Repository<Cart> ,ICartRepository
    {

        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
