using E_Commerce.Domain.Models;
using E_Commerve.Persistence.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace E_Commerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }

        public IActionResult Details(int ProductId)
        {
            var shoppingCart = new ShoppingCart
            {
                Product = _unitOfWork.ProductRepository.Get(w => w.Id == ProductId, includeProperties: "Category"),
                Count = 1,
                ProductId = ProductId
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
                cartFromDb.Count += shoppingCart.Count;
            else
                _unitOfWork.ShoppingCartRepository.Add(shoppingCart);

            TempData["success"] = "Cart updated successfully";
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}