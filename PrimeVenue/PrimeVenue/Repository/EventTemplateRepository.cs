using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public class EventTemplateRepository : IEventTemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public EventTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<EventTemplate> GetAll()
        {
            return _context.EventTemplates
                           .Include(t => t.EventRequest)
                           .Include(t => t.TemplateVendors)
                           .ThenInclude(tv => tv.VendorService)
                           .ToList();
        }

        public EventTemplate GetById(int id)
        {
            return _context.EventTemplates
                           .Include(t => t.EventRequest)
                           .Include(t => t.TemplateVendors)
                           .ThenInclude(tv => tv.VendorService)
                           .ThenInclude(v => v.Vendor)
                           .FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<EventTemplate> GetByEventRequest(int eventRequestId)
        {
            return _context.EventTemplates
                           .Where(t => t.EventRequestId == eventRequestId)
                           .Include(t => t.TemplateVendors)
                           .ThenInclude(tv => tv.VendorService)
                           .ToList();
        }

        public void Add(EventTemplate template)
        {
            _context.EventTemplates.Add(template);
            _context.SaveChanges();
        }

        public void Update(EventTemplate template)
        {
            _context.EventTemplates.Update(template);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.EventTemplates.FirstOrDefault(t => t.Id == id);
            if (existing != null)
            {
                _context.EventTemplates.Remove(existing);
                _context.SaveChanges();
            }
        }

        // ---------- TemplateVendor Management ----------
        public void AddVendorToTemplate(TemplateVendor templateVendor)
        {
            _context.TemplateVendors.Add(templateVendor);
            _context.SaveChanges();
        }

        public void RemoveVendorFromTemplate(int templateVendorId)
        {
            var existing = _context.TemplateVendors.FirstOrDefault(tv => tv.Id == templateVendorId);
            if (existing != null)
            {
                _context.TemplateVendors.Remove(existing);
                _context.SaveChanges();
            }
        }

        public IEnumerable<TemplateVendor> GetVendorsForTemplate(int eventTemplateId)
        {
            return _context.TemplateVendors
                           .Include(tv => tv.VendorService)
                           .Where(tv => tv.EventTemplateId == eventTemplateId)
                           .ToList();
        }
    }
}
