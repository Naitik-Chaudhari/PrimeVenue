using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public class VendorServiceRepository : IVendorServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public VendorServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<VendorService> GetAll()
        {
            return _context.VendorServices
                           .Include(v => v.Vendor)
                           .OrderByDescending(v => v.Rating)
                           .ToList();
        }

        public VendorService GetById(int id)
        {
            return _context.VendorServices
                           .Include(v => v.Vendor)
                           .FirstOrDefault(v => v.Id == id);
        }

        public IEnumerable<VendorService> GetByVendor(string vendorId)
        {
            return _context.VendorServices
                           .Where(v => v.VendorId == vendorId)
                           .OrderBy(v => v.ServiceType)
                           .ToList();
        }

        public void Add(VendorService service)
        {
            _context.VendorServices.Add(service);
            _context.SaveChanges();
        }

        public void Update(VendorService service)
        {
            _context.VendorServices.Update(service);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.VendorServices.FirstOrDefault(v => v.Id == id);
            if (existing != null)
            {
                _context.VendorServices.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
