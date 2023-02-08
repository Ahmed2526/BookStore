using DAL.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ModelsLayer.ViewModel;
using System.Security.Claims;

namespace WebLayer.Areas.Home.Controllers
{
    [Area("Home")]
    [Authorize]
    public class CartController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;
        public  ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
		public IActionResult Index()
        {
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCarts.GetAll
                (e => e.ApplicationUserId == claim.Value, new[] { "Product" })
            };

			foreach (var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
					cart.Product.Price50, cart.Product.Price100);
				ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
				
			}

			return View(ShoppingCartVM);
        }

		[ValidateAntiForgeryToken]
		public IActionResult Summary()
		{


			return View();
		}

        public IActionResult Plus(int cartId)
        {
			var cart = _unitOfWork.ShoppingCarts.Find(cartId);

			cart.Count += 1;
			_unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

		public IActionResult minus(int cartId)
		{
			var cart = _unitOfWork.ShoppingCarts.Find(cartId);

			if (cart.Count == 1)
			{
				_unitOfWork.ShoppingCarts.Remove(cart);
				_unitOfWork.Save();
			}
			else
			{
				cart.Count -= 1;
				_unitOfWork.Save();
			}

			return RedirectToAction(nameof(Index));
		}

		public IActionResult remove(int cartId)
		{
			var cart = _unitOfWork.ShoppingCarts.Find(cartId);

			if (cart!=null)
			{
				_unitOfWork.ShoppingCarts.Remove(cart);
				_unitOfWork.Save();
			}

			return RedirectToAction(nameof(Index));
		}



		private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
		{
			if (quantity <= 50)
			{
				return price;
			}
			else
			{
				if (quantity <= 100)
				{
					return price50;
				}
				return price100;
			}
		}

	}
}
