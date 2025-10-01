using E_Commerce.DataAccess;
using E_Commerce.Models;
using E_Commerce.Repositories;
using E_Commerce.Repositories.IRepositories;


namespace E_Commerce511.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>,IOrderItemRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateRange(List<OrderItem> orderItems)
        {
            dbContext.AddRange(orderItems);
        }


    }
}