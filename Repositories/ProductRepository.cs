using E_Commerce.DataAccess;
using E_Commerce.Models;
using E_Commerce.Repositories.IRepositories;

namespace E_Commerce.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
