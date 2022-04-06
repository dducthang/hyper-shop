using HyperShop.DataAccess.Data;
using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace HyperShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public CartController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public IActionResult Index()
        {
            if (!User.IsInRole(SD.Role_User))
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == userId);
            var cartItems = _unitOfWork.CartDetail.GetAllByCartId(cart.Id, "ProductVariation").ToList();

            var cartVm = new CartVM
            {
                CartDetails = cartItems,
                Images = new()
            };
            foreach(var item in cartItems)
            {
                var image = _unitOfWork.PrimaryImage.GetFirstOrDefault(i => i.Color_Id == item.ProductVariation.Color_Id && i.Product_Id == item.ProductVariation.Product_Id);
                cartVm.Images.Add(image);
            }
            return View(cartVm);
        }

        #region API CALLS
        [HttpPost]
        public IActionResult AddToCart([FromBody]AddToCartObj obj)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == obj.productId);
            if (User.IsInRole(SD.Role_User))
            {
                //check if user was logged in
                //var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //get user's cart
                var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == userId);

                //if cart null, create new cart with user's Id
                if (cart == null)
                {
                    _unitOfWork.Cart.Add(new Cart
                    {
                        Total = 0,
                        User_Id = userId,
                        Content = "Nothing"
                    });
                    _unitOfWork.Save();
                    cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == userId);
                }

                //get all product variations with known productId and colorId
                var productVariations = _unitOfWork.ProductVariation.GetAllByProdIdAndColor(obj.productId, obj.colorId, "Size,Color");
                // get all existed item in user's cart
                var existedCartItems = _unitOfWork.CartDetail
                                                 .GetAllByCartId(cart.Id, "ProductVariation")
                                                 .Where(i => i.ProductVariation.Color_Id == obj.colorId && i.ProductVariation.Product_Id == obj.productId)
                                                 .ToList();

                foreach (var item in obj.sizeList)
                {
                    var productVariation = productVariations.SingleOrDefault(p => p.Size.SizeValue == item);

                    var existedItem = existedCartItems.FirstOrDefault(e => e.ProductVariation_Id == productVariation.Id);
                    if (existedItem != null)
                    {
                        existedItem.Quantity += 1;
                        _unitOfWork.CartDetail.Update(existedItem);
                    }
                    else
                    {
                        var cartItem = new CartDetail
                        {
                            ProductVariation_Id = productVariation.Id,
                            Cart_Id = cart.Id,
                            Quantity = 1
                        };
                        _unitOfWork.CartDetail.Add(cartItem);
                    }
                }
                _unitOfWork.Save();
            }
            else
            {
                return Json(new { message = "Some thing went wrong" });
            }
           
            return Json(new { message="Add product to cart successfully"});
        }

        [HttpPost]
        public IActionResult UpdateCartItem([FromBody]UpdateCartObj obj)
        {
            // Update cart detail in database
            var temp = obj;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == userId);
            var cartDetail = _unitOfWork.CartDetail.GetFirstOrDefault(cd => cd.Cart_Id == cart.Id && cd.ProductVariation_Id == obj.VariationId, "ProductVariation");
            obj.Total -= (int)((cartDetail.Quantity - obj.Quantity) * cartDetail.ProductVariation.Product.Price);

            cartDetail.Quantity = obj.Quantity;
            _unitOfWork.CartDetail.Update(cartDetail);
            _unitOfWork.Save();

            // Calculate value for display in view
            var subTotal = cartDetail.ProductVariation.Product.Price * cartDetail.Quantity;

            return Json(new { cartDetail, total=obj.Total, subTotal });
        }

        [HttpPost]
        public IActionResult DeleteCartItem([FromBody]UpdateCartObj obj)
        {
            var cartDetail = _unitOfWork.CartDetail.GetFirstOrDefault(cd => cd.ProductVariation_Id == obj.VariationId, "ProductVariation");
            obj.Total -= (int)cartDetail.ProductVariation.Product.Price * cartDetail.Quantity;
            _unitOfWork.CartDetail.Remove(cartDetail);
            _unitOfWork.Save();
            return Json(new { total = obj.Total, variationId = obj.VariationId});
        }
        #endregion

    }
}
