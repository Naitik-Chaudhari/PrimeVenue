using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------- Category Methods ----------
        public IEnumerable<Category> GetAll()
        {
            return _context.Categories
                           .Include(c => c.SubCategories)
                           .OrderBy(c => c.Name)
                           .ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories
                           .Include(c => c.SubCategories)
                           .FirstOrDefault(c => c.Id == id);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (existing != null)
            {
                _context.Categories.Remove(existing);
                _context.SaveChanges();
            }
        }

        // ---------- SubCategory Methods ----------
        public IEnumerable<SubCategory> GetSubCategoriesByCategory(int categoryId)
        {
            return _context.SubCategories
                           .Where(sc => sc.CategoryId == categoryId)
                           .OrderBy(sc => sc.Name)
                           .ToList();
        }

        public SubCategory GetSubCategoryById(int id)
        {
            return _context.SubCategories.FirstOrDefault(sc => sc.Id == id);
        }

        public void AddSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Add(subCategory);
            _context.SaveChanges();
        }

        public void UpdateSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Update(subCategory);
            _context.SaveChanges();
        }

        public void DeleteSubCategory(int id)
        {
            var existing = _context.SubCategories.FirstOrDefault(sc => sc.Id == id);
            if (existing != null)
            {
                _context.SubCategories.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
