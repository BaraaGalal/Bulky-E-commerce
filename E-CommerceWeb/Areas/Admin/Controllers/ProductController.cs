using E_Commerce.Domain.Models;
using E_Commerce.Domain.ViewModels;
using E_Commerce.Utility;
using E_Commerve.Persistence.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
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

            productVM.Product = _unitOfWork.ProductRepository.Get(w => w.Id == id, includeProperties: "ProductImages");
            return View(productVM);
        }
        [HttpPost]
        public IActionResult UpSert(ProductVM productVM, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                    _unitOfWork.ProductRepository.Add(productVM.Product);
                else
                    _unitOfWork.ProductRepository.Update(productVM.Product);

                _unitOfWork.Save();

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {

                    foreach (IFormFile file in files)
                    {

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);
                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var filestream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };

                        if (productVM.Product.ProductImages == null)
                            productVM.Product.ProductImages = new List<ProductImage>();

                        productVM.Product.ProductImages.Add(productImage);
                    }

                    _unitOfWork.ProductRepository.Update(productVM.Product);
                    _unitOfWork.Save();
                }

                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null) return NotFound();

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }
            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product removed successfully";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteImage(int imageId)
        {
            var productImage = _unitOfWork.ProductImageRepository.GetById(imageId);
            int productId = productImage.ProductId;

            if (productImage == null) return NotFound();

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                  productImage.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.ProductImageRepository.Remove(productImage);
            _unitOfWork.Save();
            TempData["success"] = "Image removed successfully";

            return RedirectToAction(nameof(UpSert), new { id = productId });
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }
        #endregion
    }
}
