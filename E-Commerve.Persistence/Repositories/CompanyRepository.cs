using E_Commerce.Domain.Models;
using E_Commerce.Persistence.Data;
using E_Commerve.Persistence.Repositories.IRepositories;

namespace E_Commerve.Persistence.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }


    }
}
