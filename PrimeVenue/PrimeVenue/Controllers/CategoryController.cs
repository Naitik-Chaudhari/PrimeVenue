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
        public IActionResult SubCategories(int categoryId)
        {
            var subCategories = _categoryRepo.GetSubCategoriesByCategory(categoryId);
            ViewBag.CategoryId = categoryId;
            return View(subCategories);
        }

        public IActionResult CreateSubCategory(int categoryId)
        {
            ViewBag.CategoryId = categoryId;
            return View();
        }

        // ---------------- Create SubCategory (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSubCategory(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.AddSubCategory(subCategory);
                return RedirectToAction("SubCategories", new { categoryId = subCategory.CategoryId });
            }
            ViewBag.CategoryId = subCategory.CategoryId;
            return View(subCategory);
        }
    }
}
