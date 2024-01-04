using E_Commerce.Domain.Models;
using E_Commerce.Utility;
using E_Commerve.Persistence.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var companies = _unitOfWork.CompanyRepository.GetAll().ToList();
            return View(companies);
        }

        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.CompanyRepository.Get(u => u.Id == id);
                return View(companyObj);
            }

        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.CompanyRepository.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.CompanyRepository.Update(CompanyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                return View(CompanyObj);
            }
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (!ModelState.IsValid) return View();

            _unitOfWork.CompanyRepository.Update(company);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var company = _unitOfWork.CompanyRepository.GetById(id);
            if (company == null) return NotFound();

            _unitOfWork.CompanyRepository.Remove(company);
            _unitOfWork.Save();
            TempData["success"] = "Category removed successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.CompanyRepository.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

    }
}
