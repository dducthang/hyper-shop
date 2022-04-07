using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace HyperShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == userId);
            if (cart == null)
            {
                _unitOfWork.Cart.Add(new Models.Cart
                {
                    User_Id = userId
                });

                _unitOfWork.Save();
            }
            //create cart session
            if (HttpContext.Session.GetString("CartSession")==null)
            {
                var cartSession = new CartVM 
                {
                    CartDetails=new(),
                    Images=new()
                };
                HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(cartSession));
            }

            var products = _unitOfWork.Product.GetAll().ToList();
            var variations = _unitOfWork.ProductVariation.GetAll().ToList();
            products = products.Where(x => variations.Select(v => v.Product_Id).Contains(x.Id)).Take(5).ToList();
            return View(products);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API CALLs
        [HttpGet]
        public IActionResult GetAvailableProducts() 
        {
            var products = _unitOfWork.Product.GetAll().ToList();
            var variations = _unitOfWork.ProductVariation.GetAll().ToList();
            products = products.Where(x => variations.Select(v => v.Product_Id).Contains(x.Id)).ToList();
            return Json(new { products = products });
        }
        #endregion
    }
}