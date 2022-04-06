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

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                BrandList = _unitOfWork.Brand.GetAll().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (id== null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, includeProperties:"Category,Brand");
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var imageWorking = new ImageWorking(_hostEnvironment);
                if (file != null)
                {
                    string imagePath = @"\" + imageWorking.SaveImage(@"images\products\", file, obj.Product.MainImage);
                    obj.Product.MainImage = imagePath;
                    imageWorking.Image_resize(imagePath, imagePath);
                }

                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    TempData["success"] = "Product Updated Successfully";
                }
                _unitOfWork.Save();
                return Redirect("/Admin/ProductVariation/Index?productId=" + obj.Product.Id);
            }
            return View(obj);
        }

        public IActionResult CreateVariation(int? productId)
        {
            TempData["productId"] = productId;
            return RedirectToAction("Create", "ProductVariation", productId);
        }

        /*public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }*/

        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }*/

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties:"Brand,Category");
            return Json(new {data=products});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            var variations = _unitOfWork.ProductVariation.GetAllByProductId((int)id, "Color");
            var oddImages = _unitOfWork.Image.GetAllByProductId((int)id, "Color");
            var primaryImages = _unitOfWork.PrimaryImage.GetAllByProductId((int)id, "Color");

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            //Delete odd images
            foreach(var item in oddImages)
            {
                var oddImagePath = Path.Combine(_hostEnvironment.WebRootPath, item.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oddImagePath))
                {
                    System.IO.File.Delete(oddImagePath);
                }
            }

            //Delete primary image
            foreach (var item in primaryImages)
            {
                var primaryImagePath = Path.Combine(_hostEnvironment.WebRootPath, item.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(primaryImagePath))
                {
                    System.IO.File.Delete(primaryImagePath);
                }
            }

            //Delete main image
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.MainImage.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            

            _unitOfWork.ProductVariation.RemoveRange(variations);
            _unitOfWork.PrimaryImage.RemoveRange(primaryImages);
            _unitOfWork.Image.RemoveRange(oddImages);
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion

    }
}
