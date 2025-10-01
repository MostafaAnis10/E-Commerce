using E_Commerce.DataAccess;
using E_Commerce.Models;
using E_Commerce.Repositories.IRepositories;
using E_Commerce.DataAccess;
using E_Commerce.Models;
using E_Commerce.Repositories.IRepositories;


namespace E_Commerce.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
