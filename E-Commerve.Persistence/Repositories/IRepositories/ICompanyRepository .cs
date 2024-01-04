using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerve.Persistence.Repositories.IRepositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company category);
    }
}
