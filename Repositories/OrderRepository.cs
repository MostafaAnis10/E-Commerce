using E_Commerce.DataAccess;
using E_Commerce.Models;
using E_Commerce.Repositories.IRepositories;

namespace E_Commerce.Repositories
{
    public class OrderRepository:Repository<Order> , IOrderRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
