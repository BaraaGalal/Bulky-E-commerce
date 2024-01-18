using E_Commerce.Domain.Models;
using E_Commerce.Persistence.Data;
using E_Commerve.Persistence.Repositories.IRepositories;

namespace E_Commerve.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        } 
        public void Update(Product entity)
        {
            var product = _context.Products.Find(entity.Id);
            if (product != null)
            {
                product.Title = entity.Title;
                product.ISBN = entity.ISBN;
                product.Price = entity.Price;
                product.Price50 = entity.Price;
                product.ListPrice = entity.ListPrice;
                product.Price100 = entity.Price100;
                product.Description = entity.Description;
                product.CategoryId = entity.CategoryId;
                product.Author = entity.Author;
                product.ProductImages = entity.ProductImages;

                //if (entity.ImageUrl != null) 
                //    product.ImageUrl = entity.ImageUrl;
            }
        }
    }
}
