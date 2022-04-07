using HyperShop.DataAccess.Data;
using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var cartVm = new CartVM()
            {
                CartDetails = new(),
                Images = new()
            };

            if (HttpContext.Session.GetString("CartSession") == null)
            {
                HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(cartVm));
            }

            var cartSession = JsonConvert
                    .DeserializeObject<CartVM>(HttpContext.Session.GetString("CartSession"));

            if (!User.IsInRole(SD.Role_User))
            {
                foreach (var cartDetail in cartSession.CartDetails)
                {
                    cartDetail.ProductVariation = _unitOfWork.ProductVariation.GetFirstOrDefault(p => p.Id == cartDetail.ProductVariation_Id, "Size,Color,Product");
                }
                cartVm = cartSession;
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == userId);
                var cartItems = _unitOfWork.CartDetail.GetAllByCartId(cart.Id, "ProductVariation").ToList();

                cartVm.CartDetails = cartItems;
                foreach(var item in cartVm.CartDetails)
                {
                    var existedItem = cartSession.CartDetails.FirstOrDefault(i => i.ProductVariation_Id == item.ProductVariation_Id);
                    if (existedItem != null)
                    {
                        item.Quantity += existedItem.Quantity;
                        cartSession.CartDetails.Remove(existedItem);
                        cartSession.Images
                            .Remove(cartSession.Images.FirstOrDefault(i =>
                                                        i.Product_Id == item.ProductVariation.Product_Id
                                                        && i.Color_Id == item.ProductVariation.Color_Id));
                        _unitOfWork.CartDetail.Update(item);
                    }

                    var image = _unitOfWork.PrimaryImage
                        .GetFirstOrDefault(i =>     
                                i.Color_Id == item.ProductVariation.Color_Id 
                                && i.Product_Id == item.ProductVariation.Product_Id);

                    cartVm.Images.Add(image);
                }

                for(var i= 0; i < cartSession.CartDetails.Count; i++)
                {
                    
                    _unitOfWork.CartDetail.Add(new CartDetail
                    {
                        Cart_Id=cart.Id,
                        ProductVariation_Id = cartSession.CartDetails[i].ProductVariation_Id,
                        Quantity = cartSession.CartDetails[i].Quantity
                    });
                    cartSession.CartDetails[i].ProductVariation = _unitOfWork.ProductVariation
                                                                            .GetFirstOrDefault(it => it.Id == cartSession.CartDetails[i].ProductVariation_Id, "Size,Color,Product");
                    cartVm.CartDetails.Add(cartSession.CartDetails[i]);
                    cartVm.Images.Add(cartSession.Images[i]);
                    cartSession.CartDetails.Remove(cartSession.CartDetails[i]);
                    cartSession.Images.Remove(cartSession.Images[i]);
                    
                }
                HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(cartSession));

                _unitOfWork.Save();

            }
            return View(cartVm);
        }

        #region API CALLS
        [HttpPost]
        public IActionResult AddToCart([FromBody]AddToCartObj obj)
        {
            //var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == obj.productId);
            if (!User.IsInRole(SD.Role_User))
            {
                var cartSession = JsonConvert
                    .DeserializeObject<CartVM>(HttpContext.Session.GetString("CartSession"));

                foreach(var size in obj.sizeList)
                {
                    //get all info of product variation gonna be added to CartSession
                    var productVariation = _unitOfWork.ProductVariation
                            .GetAllByProdIdAndColor(obj.productId, obj.colorId, "Size,Color")
                            .FirstOrDefault(p => p.Size.SizeValue == size);

                    //check if product variation has been existed in CartSession
                    var existedItem = cartSession.CartDetails
                        .FirstOrDefault(i=>i.ProductVariation_Id == productVariation.Id);

                    //if has been existed => increase the quantity
                    if (existedItem != null)
                    {
                        existedItem.Quantity += 1;
                    }
                    else
                    {
                        var cartDetail = new CartDetail
                        {
                            ProductVariation_Id = productVariation.Id,
                            Quantity = 1
                        };
                        
                        cartSession.CartDetails.Add(new CartDetail
                        {
                            ProductVariation_Id = productVariation.Id,
                            Quantity = 1,
                            Cart_Id = -1
                        });

                        var image = _unitOfWork.PrimaryImage.GetFirstOrDefault(i =>
                                    i.Product_Id == productVariation.Product_Id
                                    && i.Color_Id == productVariation.Color_Id);
                        cartSession.Images.Add(image);
                    }
                }
                HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(cartSession));

            }
            else if (User.IsInRole(SD.Role_User))
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

                foreach (var size in obj.sizeList)
                {
                    var productVariation = productVariations
                                            .SingleOrDefault(p => p.Size.SizeValue == size);

                    var existedItem = existedCartItems
                                         .FirstOrDefault(e => e.ProductVariation_Id == productVariation.Id);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartDetail = new CartDetail();
            if (userId == null)
            {
                var cartSession = JsonConvert
                   .DeserializeObject<CartVM>(HttpContext.Session.GetString("CartSession"));

                cartDetail = cartSession.CartDetails.FirstOrDefault(i => i.ProductVariation_Id == obj.VariationId);
                cartDetail.ProductVariation = _unitOfWork.ProductVariation.GetFirstOrDefault(i => i.Id == cartDetail.ProductVariation_Id, "Size,Color,Product");
                obj.Total -= (int)((cartDetail.Quantity - obj.Quantity) * (cartDetail.ProductVariation.Product.Price));
                cartDetail.Quantity = obj.Quantity;
                HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(cartSession));

            }
            else
            {
                var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.User_Id == userId);
                cartDetail = _unitOfWork.CartDetail.GetFirstOrDefault(cd => cd.Cart_Id == cart.Id && cd.ProductVariation_Id == obj.VariationId, "ProductVariation");
                obj.Total -= (int)((cartDetail.Quantity - obj.Quantity) * cartDetail.ProductVariation.Product.Price);

                cartDetail.Quantity = obj.Quantity;
                _unitOfWork.CartDetail.Update(cartDetail);
                _unitOfWork.Save();

                // Calculate value for display in view

            }

            var subTotal = cartDetail.ProductVariation.Product.Price * cartDetail.Quantity;

            return Json(new { cartDetail, total=obj.Total, subTotal });
        }

        [HttpPost]
        public IActionResult DeleteCartItem([FromBody]UpdateCartObj obj)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartDetail = new CartDetail();
            if (userId == null)
            {
                var cartSession = JsonConvert
                   .DeserializeObject<CartVM>(HttpContext.Session.GetString("CartSession"));

                cartDetail = cartSession.CartDetails.FirstOrDefault(i => i.ProductVariation_Id == obj.VariationId);
                cartDetail.ProductVariation = _unitOfWork.ProductVariation.GetFirstOrDefault(i => i.Id == cartDetail.ProductVariation_Id, "Size,Color,Product");
                obj.Total -= (int)((cartDetail.Quantity - obj.Quantity) * (cartDetail.ProductVariation.Product.Price));

                int index = cartSession.CartDetails.IndexOf(cartDetail);
                cartSession.CartDetails.Remove(cartSession.CartDetails[index]);
                cartSession.Images.Remove(cartSession.Images[index]);
                HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(cartSession));
            }
            else
            {
                cartDetail = _unitOfWork.CartDetail.GetFirstOrDefault(cd => cd.ProductVariation_Id == obj.VariationId, "ProductVariation");
                obj.Total -= (int)cartDetail.ProductVariation.Product.Price * cartDetail.Quantity;
                _unitOfWork.CartDetail.Remove(cartDetail);
                _unitOfWork.Save();
            }
            return Json(new { total = obj.Total, variationId = obj.VariationId});
        }
        #endregion

    }
}
