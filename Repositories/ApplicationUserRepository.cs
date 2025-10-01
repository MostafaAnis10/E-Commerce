using E_Commerce.DataAccess;
using E_Commerce.Models;
using E_Commerce.Repositories.IRepositories;

namespace E_Commerce.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext applicationDb) : base(applicationDb)
        {
        }
    }
}
