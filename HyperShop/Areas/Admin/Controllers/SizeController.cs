using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SizeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SizeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Size> items = _unitOfWork.Size.GetAll();
            return View(items);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Size obj)
        {
            Size objFromDb = _unitOfWork.Size.GetFirstOrDefault(x => x.SizeValue == obj.SizeValue);
            if (objFromDb != null)
            {
                ModelState.AddModelError("sizeValue", "This size has been existed");
            }
            if (ModelState.IsValid)
            {
                
                _unitOfWork.Size.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Size add successfully";
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
            Size objFromDb = _unitOfWork.Size.GetFirstOrDefault(x => x.Id == id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            return View(objFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Size obj)
        {
            Size objFromDb = _unitOfWork.Size.GetFirstOrDefault(x => x.SizeValue == obj.SizeValue);
            if (objFromDb != null)
            {
                ModelState.AddModelError("sizeValue", "This size has been existed");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Size.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Size updated successfully";
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
            var objFromDbFirst = _unitOfWork.Size.GetFirstOrDefault(u => u.Id == id);

            if (objFromDbFirst == null)
            {
                return NotFound();
            }

            return View(objFromDbFirst);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Size.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Size.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Size deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
