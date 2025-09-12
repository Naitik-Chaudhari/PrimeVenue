using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public interface IVendorServiceRepository
    {
        IEnumerable<VendorService> GetAll();
        VendorService GetById(int id);
        IEnumerable<VendorService> GetByVendor(string vendorId);
        void Add(VendorService service);
        void Update(VendorService service);
        void Delete(int id);
    }
}
