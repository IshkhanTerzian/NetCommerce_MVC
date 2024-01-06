using Microsoft.AspNetCore.Mvc;
using BookBazaar.DataAccess.Data;
using BookBazaar.DataAccess.Repository.IRepository;
using BookBazaar.Models;

namespace BookBazaar.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository db)
        {
            _categoryRepository = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepository.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _categoryRepository.Add(obj);
                _categoryRepository.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDB = _categoryRepository.Get(u => u.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(obj);
                _categoryRepository.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDB = _categoryRepository.Get(u => u.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _categoryRepository.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _categoryRepository.Remove(obj);
            _categoryRepository.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
