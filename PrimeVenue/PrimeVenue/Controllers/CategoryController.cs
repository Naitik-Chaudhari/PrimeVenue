using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeVenue.Model;
using PrimeVenue.Repository;

namespace PrimeVenue.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // Show all categories
        public IActionResult Index()
        {
            var categories = _categoryRepo.GetAll();
            return View(categories);
        }

        // ---------------- Create Category (GET)
        public IActionResult Create()
        {
            return View();
        }

        // ---------------- Create Category (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // Show subcategories for a category
        // Show subcategories for a category
        public IActionResult SubCategories(int categoryId)
        {
            var subCategories = _categoryRepo.GetSubCategoriesByCategory(categoryId);
            var category = _categoryRepo.GetById(categoryId); // optional, for name display
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryName = category?.Name; // optional
            return View(subCategories); // <-- This must point to SubCategories.cshtml
        }

<<<<<<< HEAD
=======

        public IActionResult CreateSubCategory(int categoryId)
        {
            var model = new SubCategory { CategoryId = categoryId, Name = string.Empty };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSubCategory(SubCategory subCategory)
        {
            // Prevent validation error for navigation property 'Category' (not posted from form)
            ModelState.Remove(nameof(SubCategory.Category));

            if (ModelState.IsValid)
            {
                _categoryRepo.AddSubCategory(subCategory);
                return RedirectToAction("SubCategories", new { categoryId = subCategory.CategoryId });
            }
            ViewBag.CategoryId = subCategory.CategoryId;
            return View(subCategory);
        }
>>>>>>> 8bb57046f109f047f3ad315d824b1cccf706119b
    }
}
