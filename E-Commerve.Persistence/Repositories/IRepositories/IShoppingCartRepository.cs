using E_Commerce.Domain.Models;

namespace E_Commerve.Persistence.Repositories.IRepositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
    }
}
