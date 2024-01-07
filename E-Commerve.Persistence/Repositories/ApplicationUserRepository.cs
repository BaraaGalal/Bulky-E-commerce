using E_Commerce.Domain.Models;
using E_Commerce.Persistence.Data;
using E_Commerve.Persistence.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerve.Persistence.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
       private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



    }
}
