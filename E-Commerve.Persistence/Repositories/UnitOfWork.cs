using E_Commerce.Persistence.Data;
using E_Commerve.Persistence.Repositories.IRepositories;

namespace E_Commerve.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository CategoryRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }
        public IOrderDetailRepository OrderDetailRepository { get; private set; }
        public IProductImageRepository ProductImageRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context);
            CompanyRepository = new CompanyRepository(context);
            ShoppingCartRepository = new ShoppingCartRepository(context);
            ApplicationUserRepository = new ApplicationUserRepository(context);
            OrderHeaderRepository = new OrderHeaderRepository(context);
            OrderDetailRepository = new OrderDetailRepository(context);
            ProductImageRepository = new ProductImageRepository(context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
