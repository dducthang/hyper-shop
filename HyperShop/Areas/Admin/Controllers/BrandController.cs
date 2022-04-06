using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HyperShop.Utility.Class;

namespace HyperShop.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Brand> brands = _unitOfWork.Brand.GetAll();
            return View(brands);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Brand obj)
        {
            Brand objFromDb = _unitOfWork.Brand.GetFirstOrDefault(x => x.Name == obj.Name);
            if (objFromDb != null)
            {
                ModelState.AddModelError("name", "This brand has been existed");
            }
            if (ModelState.IsValid)
            {
                
                _unitOfWork.Brand.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Brand Add successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if(id== null || id == 0)
            {
                return NotFound();
            }
            Brand objFromDb = _unitOfWork.Brand.GetFirstOrDefault(x => x.Id == id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            return View(objFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Brand obj)
        {
            Brand objFromDb = _unitOfWork.Brand.GetFirstOrDefault(x => x.Name == obj.Name);
            if (objFromDb != null)
            {
                ModelState.AddModelError("name", "This Brand has been existed");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Brand.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Brand updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDbFirst = _unitOfWork.Brand.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Brand.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Brand.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Brand deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
