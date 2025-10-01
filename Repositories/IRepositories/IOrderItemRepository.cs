using E_Commerce.Models;

namespace E_Commerce.Repositories.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        void CreateRange(List<OrderItem> orderItems);
    }
}
