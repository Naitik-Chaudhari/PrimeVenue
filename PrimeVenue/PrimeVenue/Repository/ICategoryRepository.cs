using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);

        // SubCategory methods
        IEnumerable<SubCategory> GetSubCategoriesByCategory(int categoryId);
        SubCategory GetSubCategoryById(int id);
        void AddSubCategory(SubCategory subCategory);
        void UpdateSubCategory(SubCategory subCategory);
        void DeleteSubCategory(int id);
    }
}
