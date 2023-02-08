using DAL.Repository.IRepository;
using DAL.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace WebLayer.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var Productlst = _unitOfWork.Products.GetAll(e => e.ID > 0, new[] { "Category", "Cover" });
            return View(Productlst);
        }
        public ActionResult Details(int? productId)
        {
            if (productId != null)
            {
                ShoppingCart cart = new ShoppingCart()
                {
                    ProductId=(int)productId,
                    Product = _unitOfWork.Products.Find(e => e.ID == productId, new[] { "Category", "Cover" })
                   ,Count=1
                };

                return View(cart);
            }
            return NotFound();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Details(ShoppingCart Cart)
        {
            var claimIdentity =(ClaimsIdentity) User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Cart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCarts.Find
                (e => e.ApplicationUserId == claim.Value && e.ProductId == Cart.ProductId);

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCarts.Add(Cart);
                _unitOfWork.Save();
            }
            else
            {
                cartFromDb.Count += Cart.Count;
                _unitOfWork.Save();
            }

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