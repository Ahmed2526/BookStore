using DAL.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;
using NuGet.Packaging.Signing;
using System.Data;
using System.Net;

namespace WebLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;

            _webHostEnvironment = webHostEnvironment;
        }
        public ActionResult Index()
        {
            return View(_unitOfWork.Products.GetAll(e => e.ID > 0, new[] { "Category", "Cover" }));
           // return View(_unitOfWork.Products.GetAll().ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id != null) 
            {
                var selected = _unitOfWork.Products.Find(e => e.ID == id, new[] { "Category", "Cover" });
                return View(selected);
            }
            return NotFound();
        }

        public ActionResult Create()
        {
            #region SorryButLazyToMakeAclass
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Categories.GetAll()
                .Select(e => new SelectListItem { Text = e.Name, Value = e.ID.ToString() });

            IEnumerable<SelectListItem> CoverList = _unitOfWork.Covers.GetAll()
                .Select(e => new SelectListItem { Text = e.Type, Value = e.ID.ToString() });

            ViewBag.CategoryList = CategoryList;
            ViewBag.CoverList = CoverList;
            #endregion

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, IFormFile? file)
        {
            #region Validating extension_Demo
            //var extensionn = Path.GetExtension(file.FileName);

            //if (extensionn != ".jpg" && extensionn != ".jpeg" && extensionn != ".gif" && extensionn != ".png")
            //{
            //    ModelState.AddModelError("ImageUrl", "Invalid Photo");

            //    #region SorryButLazyToMakeAclass
            //    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Categories.GetAll()
            //   .Select(e => new SelectListItem { Text = e.Name, Value = e.ID.ToString() });

            //    IEnumerable<SelectListItem> CoverList = _unitOfWork.Covers.GetAll()
            //        .Select(e => new SelectListItem { Text = e.Type, Value = e.ID.ToString() });

            //    ViewBag.CategoryList = CategoryList;
            //    ViewBag.CoverList = CoverList;
            //    #endregion
            //}
            #endregion

            if (ModelState.IsValid)
            {
                var PathRoot = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".gif" || extension == ".png")
                    {
                        var FileNewName = Guid.NewGuid().ToString();
                        var upload = Path.Combine(PathRoot, @"Images\Product_img");

                        using (var filestream = new FileStream(Path.Combine(upload, FileNewName + extension), FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }
                        product.ImageUrl = @"\Images\Product_img\" + FileNewName + extension;

                        _unitOfWork.Products.Add(product);
                        _unitOfWork.Save();

                        TempData["Success"] = "Product Created Successfully";

                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("ImageUrl", "InvalidPhoto!");
                    #region SorryButLazyToMakeAClass
                    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Categories.GetAll()
                   .Select(e => new SelectListItem { Text = e.Name, Value = e.ID.ToString() });

                    IEnumerable<SelectListItem> CoverList = _unitOfWork.Covers.GetAll()
                        .Select(e => new SelectListItem { Text = e.Type, Value = e.ID.ToString() });

                    ViewBag.CategoryList = CategoryList;
                    ViewBag.CoverList = CoverList;
                    #endregion
                }
            }
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id!=null)
            {
                var selectd = _unitOfWork.Products.Find((int)id);

                #region SorryButLazyToMakeAclass
                IEnumerable<SelectListItem> CategoryList = _unitOfWork.Categories.GetAll()
               .Select(e => new SelectListItem { Text = e.Name, Value = e.ID.ToString() });

                IEnumerable<SelectListItem> CoverList = _unitOfWork.Covers.GetAll()
                    .Select(e => new SelectListItem { Text = e.Type, Value = e.ID.ToString() });

                ViewBag.CategoryList = CategoryList;
                ViewBag.CoverList = CoverList;
                #endregion

                return View(selectd);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var PathRoot = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    //Deleting old photo
                    if (product.ImageUrl != null)
                    {
                        var oldimagepath = Path.Combine(PathRoot, product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimagepath))
                        {
                            System.IO.File.Delete(oldimagepath);
                        }
                    }
                    var extension = Path.GetExtension(file.FileName);
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".gif" || extension == ".png")
                    {
                        var FileNewName = Guid.NewGuid().ToString();
                        var upload = Path.Combine(PathRoot, @"Images\Product_img");

                        using (var filestream = new FileStream(Path.Combine(upload, FileNewName + extension), FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }

                        product.ImageUrl = @"\Images\Product_img\" + FileNewName + extension;

                        _unitOfWork.Products.Update(product);
                        _unitOfWork.Save();

                        TempData["Success"] = "Product Updated Successfully";

                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("ImageUrl", "InvalidPhoto!");
                    #region SorryButLazyToMakeAClass
                    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Categories.GetAll()
                   .Select(e => new SelectListItem { Text = e.Name, Value = e.ID.ToString() });

                    IEnumerable<SelectListItem> CoverList = _unitOfWork.Covers.GetAll()
                        .Select(e => new SelectListItem { Text = e.Type, Value = e.ID.ToString() });

                    ViewBag.CategoryList = CategoryList;
                    ViewBag.CoverList = CoverList;
                    #endregion
                }
            }
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var selected = _unitOfWork.Products.Find(e => e.ID == id, new[] { "Category", "Cover" });
                return View(selected);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var selected = _unitOfWork.Products.Find(id);

                if (selected == null)
                {
                    return NotFound();
                }
                if (selected.ImageUrl != null)
                {
                    var PathRoot = _webHostEnvironment.WebRootPath;
                    var oldimagepath = Path.Combine(PathRoot, selected.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldimagepath))
                    {
                        System.IO.File.Delete(oldimagepath);
                    }
                }
                _unitOfWork.Products.Remove(selected);
                _unitOfWork.Save();

                TempData["Success"] = "Product Deleted Successfully";
                return RedirectToAction("index");
            }
            catch
            {
                return View(_unitOfWork.Products.Find(id));
            }
        }
    }
}
