using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

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
            var products = _unitOfWork.Product.GetAll().ToList();
            var variations = _unitOfWork.ProductVariation.GetAll().ToList();
            products = products.Where(x => variations.Select(v => v.Product_Id).Contains(x.Id)).Take(5).ToList();
            return View(products);
        }

        /*public IActionResult Privacy()
        {
            return View();
        }*/

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