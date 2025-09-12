using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public interface ITemplateVendorRepository
    {
        IEnumerable<TemplateVendor> GetAll();
        TemplateVendor GetById(int id);
        IEnumerable<TemplateVendor> GetByTemplate(int eventTemplateId);
        void UpdateStatus(int templateVendorId, string status); // Accept/Decline
    }
}
