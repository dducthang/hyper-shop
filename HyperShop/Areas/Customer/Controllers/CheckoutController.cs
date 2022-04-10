using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HyperShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles=SD.Role_User)]
    public class CheckoutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            Console.WriteLine("Login yet?");
            //get user cart
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId);
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == userId);
            var cartDetails = _unitOfWork.CartDetail.GetAllByCartId(cart.Id, "ProductVariation").ToList();

            double totalCost = 0;

            foreach (var cartDetail in cartDetails)
            {
                totalCost += cartDetail.Quantity * cartDetail.ProductVariation.Product.Price;
            }

            var cityList = _unitOfWork.CityShipCost.GetAll().Select(i => new SelectListItem()
            {
                Text = i.CityName,
                Value = i.Id.ToString()
            });

            var order = new Order
            {
                User_Id = userId,
                Receiver = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.StreetAddress,
                TotalCost = totalCost
            };

            var checkoutVm = new CheckoutVM
            {
                Order = order,
                CityList = cityList
            };

            return View(checkoutVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(CheckoutVM obj)
        {
            if (ModelState.IsValid)
            {
                var orderStatus = _unitOfWork.OrderStatus.GetFirstOrDefault(s => s.Status == SD.OrderStatus_Pending);
                obj.Order.Status_Id = orderStatus.Id;
                obj.Order.OrderDate = DateTime.Today;

                var shipCost = _unitOfWork.CityShipCost.GetFirstOrDefault(c => c.Id == obj.Order.CityShipCost_Id);
                obj.Order.TotalCost += shipCost.ShipCost;
                _unitOfWork.Order.Add(obj.Order);
                _unitOfWork.Save();

                var lastestOrder = _unitOfWork.Order.GetLastestById();

                var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == obj.Order.User_Id);
                var cartDetails = _unitOfWork.CartDetail.GetAllByCartId(cart.Id);

                foreach(var cartDetail in cartDetails)
                {
                    _unitOfWork.OrderDetail.Add(new OrderDetail
                    {
                        ProductVariation_Id = cartDetail.ProductVariation_Id,
                        Order_Id = lastestOrder.Id,
                        Quantity = cartDetail.Quantity
                    });
                }

                _unitOfWork.CartDetail.RemoveRange(cartDetails);
                _unitOfWork.Save();
                return RedirectToAction("Orders");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Orders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            var orders = _unitOfWork.Order.GetAllByUserId(userId, "CityShipCost,OrderStatus");
            return View(orders);
        }

        public IActionResult SingleOrder(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _unitOfWork.Order.GetFirstOrDefault(o => o.User_Id == userId && o.Id == orderId, "OrderStatus,CityShipCost");
            var orderDetails = _unitOfWork.OrderDetail.GetAllByOrderId(order.Id, "ProductVariation").ToList();
            var primaryImages = new List<PrimaryImage>();

            foreach(var orderDetail in orderDetails)
            {
                var image = _unitOfWork.PrimaryImage.GetFirstOrDefault(i =>
                    i.Color_Id == orderDetail.ProductVariation.Color_Id
                    && i.Product_Id == orderDetail.ProductVariation.Product_Id);

                primaryImages.Add(image);
            }
            var singleOrderVm = new SingleOrderVM
            {
                Order = order,
                OrderDetails = orderDetails,
                PrimaryImages = primaryImages
            };
            return View(singleOrderVm);
        }

        #region API CALLS
        [HttpPost]
        public IActionResult CalShipCost([FromBody]int cityId)
        {
            var city = _unitOfWork.CityShipCost.GetFirstOrDefault(c => c.Id == cityId);
            var shipCost = city.ShipCost;
            return Json(new { shipCost });
        }
        #endregion
    }
}
