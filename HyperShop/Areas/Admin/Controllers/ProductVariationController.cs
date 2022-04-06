using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using HyperShop.Utility;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HyperShop.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductVariationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductVariationController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(int? productId)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == productId, "Brand,Category");
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //GET
        public IActionResult Create(int? productId)
        {
            var existedColor = _unitOfWork.PrimaryImage
                                    .GetAllByProductId((int)productId, "Color,Product")
                                    .Select(x => x.Color_Id)
                                    .ToList();

            var variationVM = new VariationVM()
            {
                ColorList = _unitOfWork.Color.GetAll().Where(x => !existedColor.Contains(x.Id)).Select(x => new SelectListItem()
                {
                    Text = x.ColorValue,
                    Value = x.Id.ToString()
                }),
                SizeList = new List<Pair>(),
                PrimaryImage = new(),
            };
            variationVM.PrimaryImage.Product_Id = (int)productId;


            var sizes = _unitOfWork.Size.GetAll().ToList();
            foreach (var size in sizes)
            {
                variationVM.SizeList.Add(new Pair()
                {
                    Key = size.SizeValue.ToString(),
                    Value = 0
                });
            }

            TempData["productId"] = productId;
            return View(variationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VariationVM obj, IFormFile file)
        {
            if (obj.ImageList.Count != 7)
            {
                ModelState.AddModelError("imageList", "Odd image list must contain 7 images");
            }

            if (ModelState.IsValid)
            {
                var imageWorking = new ImageWorking(_hostEnvironment);

                //Create Primary Image for product variations
                if (file != null)
                {
                    string returnUrl = @"\"+imageWorking.SaveImage("images\\variations\\", file);
                    obj.PrimaryImage.ImageUrl = returnUrl;
                    imageWorking.Image_resize(returnUrl, returnUrl);
                    _unitOfWork.PrimaryImage.Add(obj.PrimaryImage);
                }

                //Create odd images for product variations
                foreach (var img in obj.ImageList)
                {
                    string returnUrl = @"\"+imageWorking.SaveImage("images\\variations\\", img);
                    imageWorking.Image_resize(returnUrl, returnUrl);
                    var newImage = new Image()
                    {
                        Color_Id = obj.PrimaryImage.Color_Id,
                        Product_Id = obj.PrimaryImage.Product_Id,
                        ImageUrl = returnUrl
                    };
                    _unitOfWork.Image.Add(newImage);
                }

                //Create product variations
                for(var i=0;i<obj.SizeList.Count;i++)
                {
                    if (obj.SizeList[i].Value != 0)
                    {
                        var size = _unitOfWork.Size.GetFirstOrDefault(x => x.SizeValue.ToString() == obj.SizeList[i].Key, null);
                        var color = _unitOfWork.Color.GetFirstOrDefault(x => x.Id == obj.PrimaryImage.Color_Id, null);
                        var productVariation = new ProductVariation()
                        {
                            Product_Id = obj.PrimaryImage.Product_Id,
                            Size_Id = size.Id,
                            Color_Id = color.Id,
                            Quantity = obj.SizeList[i].Value
                        };
                        _unitOfWork.ProductVariation.Add(productVariation);
                    }
                }
                _unitOfWork.Save();
                return Redirect("/Admin/ProductVariation/Index?productId=" + obj.PrimaryImage.Product_Id);
            }
            TempData["productId"] = obj.PrimaryImage.Product_Id;
            return View(obj);
        }

        public IActionResult Edit(int productId, int colorId)
        { 

            var variationEditVM = new VariationEditVM()
            {
                ProductVariations = _unitOfWork.ProductVariation.GetAllByProdIdAndColor(productId, colorId, "Size,Color,Product").ToList(),
                SizeList = new(),
                PrimaryImage= _unitOfWork.PrimaryImage.GetFirstOrDefault(x=>x.Product_Id==productId && x.Color_Id==colorId, "Color,Product"),
                ImageList= new()
            };

            var sizes = _unitOfWork.Size.GetAll().ToList();
            foreach (var size in sizes)
            {
                variationEditVM.SizeList.Add(new Pair()
                {
                    Key = size.SizeValue.ToString(),
                    Value = 0
                });
            }

            for (var i=0;i<variationEditVM.SizeList.Count;i++)
            {
                foreach(var variation in variationEditVM.ProductVariations)
                {
                    if (variation.Size_Id == i+1)
                    {
                        variationEditVM.SizeList[i].Value = variation.Quantity;
                    }
                }
            }

            return View(variationEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VariationEditVM obj, IFormFile? file)
        {
            if (obj.ImageList != null && obj.ImageList.Count != 7)
            {
                ModelState.AddModelError("imageList", "Odd image list must contain 7 images");
            }

            if (ModelState.IsValid)
            {
                var imageWorking = new ImageWorking(_hostEnvironment);
                if (file != null)
                {
                    string returnUrl = @"\" + imageWorking.SaveImage(@"images\variations\", file, obj.PrimaryImage.ImageUrl);

                    obj.PrimaryImage.ImageUrl = returnUrl;
                    imageWorking.Image_resize(returnUrl, returnUrl);
                    _unitOfWork.PrimaryImage.Update(obj.PrimaryImage);
                }

                if(obj.ImageList != null)
                {
                    var oldImageList = _unitOfWork.Image
                        .GetAllByProdAndColorId(obj.PrimaryImage.Product_Id, obj.PrimaryImage.Color_Id, "Color,Product")
                        .ToList();
                    for(var i= 0;i < oldImageList.Count;i++)
                    {
                        string returnUrl = @"\" + imageWorking.SaveImage(@"images\variations\", obj.ImageList[i], oldImageList[i].ImageUrl);
                        imageWorking.Image_resize(returnUrl, returnUrl);
                        oldImageList[i].ImageUrl = returnUrl;
                        _unitOfWork.Image.Update(oldImageList[i]);
                    }

                }

                for(var j=0;j< obj.ProductVariations.Count;j++)
                {
                    if(obj.SizeList[obj.ProductVariations[j].Size_Id - 1].Value == 0)
                    {
                        _unitOfWork.ProductVariation.Remove(obj.ProductVariations[j]);
                    }
                    else
                    {
                        obj.ProductVariations[j].Quantity = obj.SizeList[obj.ProductVariations[j].Size_Id - 1].Value;
                        obj.SizeList[obj.ProductVariations[j].Size_Id - 1].Value = 0;
                        _unitOfWork.ProductVariation.Update(obj.ProductVariations[j]);
                    }                        
                }


                for(var j=0;j< obj.SizeList.Count;j++)
                {
                    if (obj.SizeList[j].Value != 0)
                    {
                        _unitOfWork.ProductVariation.Add(new ProductVariation()
                        {
                            Product_Id = obj.PrimaryImage.Product_Id,
                            Size_Id = j + 1,
                            Color_Id = obj.PrimaryImage.Color_Id,
                            Quantity = obj.SizeList[j].Value
                        });
                    }
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Edit variation successfully";
                return RedirectToAction("Index", new { productId = obj.PrimaryImage.Product_Id });
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(int productId)
        {
            List<ProductVariationVM> productVariationVMs = new List<ProductVariationVM>();
            IEnumerable<ProductVariation> items = _unitOfWork.ProductVariation.GetAllByProductId(productId, "Size,Color,Product");
            foreach(var item in items)
            {
                productVariationVMs.Add(new ProductVariationVM()
                {
                    ProductVariation = item,
                    PrimaryImage = _unitOfWork.PrimaryImage.GetFirstOrDefault(x => x.Product_Id == item.Product_Id && x.Color_Id == item.Color_Id, "Product,Color")
                });
            }
            IEnumerable<PrimaryImage> primaryImageList= _unitOfWork.PrimaryImage.GetAllByProductId(productId, "Color,Product");
            return Json(new { data = primaryImageList });
        }

        [HttpDelete]
        public IActionResult Delete(int productId, int colorId)
        {
            var objsFromDb = _unitOfWork.ProductVariation.GetAllByProdIdAndColor(productId, colorId);

            var oddImages = _unitOfWork.Image.GetAllByProdAndColorId(productId, colorId);

            //a list of image will be created using this method however only one image is existed but im too lazy to write a new method
            var primaryImage = _unitOfWork.PrimaryImage
                                    .GetAllByProductId(productId)
                                    .Where(x => x.Color_Id == colorId);

            if (objsFromDb == null || primaryImage==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //only one image is existed but using foreach because of the above comment
            foreach(var item in primaryImage)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, item.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            foreach (var item in oddImages)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, item.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.Image.RemoveRange(oddImages);
            _unitOfWork.PrimaryImage.RemoveRange(primaryImage);
            _unitOfWork.ProductVariation.RemoveRange(objsFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion

    }
}
