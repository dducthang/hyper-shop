using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ColorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ColorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Color> items = _unitOfWork.Color.GetAll();
            return View(items);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Color obj)
        {
            Color objFromDb = _unitOfWork.Color.GetFirstOrDefault(x => x.ColorValue == obj.ColorValue);
            if (objFromDb != null)
            {
                ModelState.AddModelError("colorValue", "This color has been existed");
            }
            if (ModelState.IsValid)
            {
                
                _unitOfWork.Color.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Color add successfully";
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
            Color objFromDb = _unitOfWork.Color.GetFirstOrDefault(x => x.Id == id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            return View(objFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Color obj)
        {
            Color objFromDb = _unitOfWork.Color.GetFirstOrDefault(x => x.ColorValue == obj.ColorValue);
            if (objFromDb != null)
            {
                ModelState.AddModelError("colorValue", "This Color has been existed");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Color.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Color updated successfully";
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
            var objFromDbFirst = _unitOfWork.Color.GetFirstOrDefault(u => u.Id == id);

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
            var obj = _unitOfWork.Color.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Color.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Color deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
