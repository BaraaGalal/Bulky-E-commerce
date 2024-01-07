using E_Commerce.Domain.Models;
using E_Commerce.Domain.ViewModels;
using E_Commerve.Persistence.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var ShoppingCartVM = new ShoppingCartVM
            {
                ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader += (cart.Price * cart.Count);

            }
            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(w => w.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(w => w.Id == cartId);
            if (cartFromDb.Count <= 1)
                _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            else
                cartFromDb.Count -= 1;

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(w => w.Id == cartId);
            _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            return View();
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
                return shoppingCart.Product.Price;
            else
            {
                if (shoppingCart.Count <= 100)
                    return shoppingCart.Product.Price50;

                else
                    return shoppingCart.Product.Price100;
            }
        }
    }
}
