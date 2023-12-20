using E_Commerce.Domain.Models;
using E_Commerce.Domain.ViewModels;
using E_Commerve.Persistence.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }

        public IActionResult UpSert(int? id)
        {

            var productVM = new ProductVM
            {
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(w => new SelectListItem
                {
                    Text = w.Name,
                    Value = w.Id.ToString()
                }).ToList(),
                Product = new Product()
            };
            if (id == null || id == 0)
                return View(productVM);

            productVM.Product = _unitOfWork.ProductRepository.GetById(id);
            return View(productVM);
        }
        [HttpPost]
        public IActionResult UpSert(ProductVM productVM, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                productVM.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(w => new SelectListItem
                {
                    Text = w.Name,
                    Value = w.Id.ToString()
                });
                return View(productVM);
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                using(var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                productVM.Product.ImageUrl = @"\images\product\" + fileName;
            }

            if (productVM.Product.Id == 0)
                _unitOfWork.ProductRepository.Add(productVM.Product);
            else
                _unitOfWork.ProductRepository.Update(productVM.Product);
            
            _unitOfWork.Save();
            TempData["success"] = "Product added successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null) return NotFound();

            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product removed successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll() 
        {
            var products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = products});
        }
        #endregion

    }
}
