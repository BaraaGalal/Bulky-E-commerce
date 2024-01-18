using E_Commerce.Domain.Models;

namespace E_Commerve.Persistence.Repositories.IRepositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        public void Update(ApplicationUser applicationUser);
    }
}
