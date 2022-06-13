using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models.ViewModels;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HyperShop.Areas.Admin.Controllers
{
    [Area(SD.Role_Admin)]
    [Authorize(Roles =SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var orders = _unitOfWork.Order.GetAll("OrderStatus,CityShipCost,User");
            var orderStatus = _unitOfWork.OrderStatus.GetAll().Select(i => new SelectListItem
            {
                Text = i.Status,
                Value = i.Status
            });

            var orderAminVm = new Order_AdminVM
            {
                Orders = orders,
                OrderStatus = orderStatus
            };
            return View(orderAminVm);
        }

        public IActionResult Edit(int orderId)
        {
            var order = _unitOfWork.Order.GetFirstOrDefault(o => o.Id == orderId, "OrderStatus");
            var orderStatus = _unitOfWork.OrderStatus.GetAll().Select(o=>new SelectListItem
            {
                Text = o.Status,
                Value = o.Status
            });

            var editOrderVm = new EditOrderVM
            {
                Order = order,
                OrderStatus = orderStatus
            };
            return View(editOrderVm);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string orderStatus)
        {
            var orders = _unitOfWork.Order.GetAll("OrderStatus,CityShipCost,User");
            if (orderStatus != "All")
            {
                orders = orders.Where(o => o.OrderStatus.Status == orderStatus);
            }
            var sd = _unitOfWork.OrderStatus.GetAll();
            return Json(new { data = orders, sd });
        }

        [HttpPost]
        public IActionResult UpdateStatus([FromBody]UpdateStatusObj obj)
        {
            var order = _unitOfWork.Order.GetFirstOrDefault(o => o.Id == obj.OrderId, "OrderStatus");
            var status = _unitOfWork.OrderStatus.GetFirstOrDefault(s => s.Status == obj.OrderStatus);
            if (order.Status_Id == status.Id)
            {
                return Json(new { message = "Cant not update the current status", status = obj.OrderStatus, err = true });
            }
            order.Status_Id = status.Id;

            _unitOfWork.Order.Update(order);
            _unitOfWork.Save();
            return Json(new { message = "Update Order Successfully", status=obj.OrderStatus, err=false});
        }
        #endregion
    }
}
