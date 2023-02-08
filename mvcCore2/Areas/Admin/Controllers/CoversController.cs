using DAL.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.Models;
using System.Data;

namespace WebLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CoversController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoversController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            return View(_unitOfWork.Covers.GetAll().ToList());
        }

        // GET: CoversController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CoversController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoversController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cover collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Covers.Add(collection);
                    _unitOfWork.Save();
                    TempData["Success"] = "Cover Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                return View(collection);
            }
            catch
            {
                return View(collection);
            }
        }

        // GET: CoversController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CoversController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CoversController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
           
            return View(_unitOfWork.Covers.Find((int)id));
        }

        // POST: CoversController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Cover collection)
        {
            try
            {
                if (collection==null)
                {
                    return NotFound();
                }
                _unitOfWork.Covers.Remove(collection);
                _unitOfWork.Save();
                TempData["Success"] = "Cover Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
