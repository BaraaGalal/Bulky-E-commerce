using E_Commerce.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Domain.ViewModels
{
    public class RoleManagmentVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
