using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        public IActionResult GetAll()
        {
            var orders = _unitOfWork.Order.GetAll("OrderStatus");
            var pendingOrders = orders.All(o => o.OrderStatus.Status == SD.OrderStatus_Pending);
            return View();
        }
        #endregion
    }
}
