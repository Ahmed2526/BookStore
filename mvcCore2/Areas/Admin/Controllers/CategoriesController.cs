using DAL.Data;
using DAL.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;
using System.Data;

namespace WebLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //var all = _context.Categories.OrderBy(e => e.DisplayOrder).ToList();
            var all = _unitOfWork.Categories.GetAll().ToList();
            return View(all);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            //var selected = _context.Categories.Find(id);
            var selected = _unitOfWork.Categories.Find((int)id);

            if (selected == null)
                return NotFound();

            return View(selected);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //_context.Add(collection);
                    //_context.SaveChanges();
                    _unitOfWork.Categories.Add(collection);
                    _unitOfWork.Save();

                    TempData["Success"] = "Category Created Successfully";

                    return RedirectToAction(nameof(Index));
                }
                return View(collection);
            }
            catch
            {
                return View(collection);
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            //var selected = _context.Categories.Find(id);
            var selected = _unitOfWork.Categories.Find((int)id);

            if (selected == null)
                return NotFound();

            return View(selected);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //_context.Update(collection);
                    //_context.SaveChanges();

                    _unitOfWork.Categories.Update(collection);
                    _unitOfWork.Save();

                    TempData["Success"] = "Category Modified Successfully";
                    return RedirectToAction(nameof(Index));
                }
                return View(collection);
            }
            catch
            {
                return View(collection);
            }
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            //var selected = _context.Categories.Find(id);
            var selected = _unitOfWork.Categories.Find((int)id);

            if (selected == null)
                return NotFound();

            return View(selected);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Category collection)
        {
            try
            {
                //var obj = _context.Categories.Find(collection.ID);
                var obj = _unitOfWork.Categories.Find(collection.ID);

                if (obj == null)
                {
                    return NotFound();
                }

                //_context.Categories.Remove(obj);
                //_context.SaveChanges();

                _unitOfWork.Categories.Remove(obj);
                _unitOfWork.Save();

                TempData["Success"] = "Category Deleted Successfully";
                return RedirectToAction("index");
            }
            catch
            {
                return View(collection);
            }
        }
      
    }
}
