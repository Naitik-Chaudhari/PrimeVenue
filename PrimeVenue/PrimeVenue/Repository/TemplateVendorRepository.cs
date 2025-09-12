using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public class TemplateVendorRepository : ITemplateVendorRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplateVendorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TemplateVendor> GetAll()
        {
            return _context.TemplateVendors
                           .Include(tv => tv.VendorService)
                           .Include(tv => tv.EventTemplate)
                           .ToList();
        }

        public TemplateVendor GetById(int id)
        {
            return _context.TemplateVendors
                           .Include(tv => tv.VendorService)
                           .Include(tv => tv.EventTemplate)
                           .FirstOrDefault(tv => tv.Id == id);
        }

        public IEnumerable<TemplateVendor> GetByTemplate(int eventTemplateId)
        {
            return _context.TemplateVendors
                           .Include(tv => tv.VendorService)
                           .Where(tv => tv.EventTemplateId == eventTemplateId)
                           .ToList();
        }

        public void UpdateStatus(int templateVendorId, string status)
        {
            var existing = _context.TemplateVendors.FirstOrDefault(tv => tv.Id == templateVendorId);
            if (existing != null)
            {
                existing.Status = status; // "Accepted" or "Declined"
                _context.SaveChanges();
            }
        }
    }
}
