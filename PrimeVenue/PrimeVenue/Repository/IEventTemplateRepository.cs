using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public interface IEventTemplateRepository
    {
        IEnumerable<EventTemplate> GetAll();
        EventTemplate GetById(int id);
        IEnumerable<EventTemplate> GetByEventRequest(int eventRequestId);
        void Add(EventTemplate template);
        void Update(EventTemplate template);
        void Delete(int id);

        // Manage Template Vendors (Vendor selections in template)
        void AddVendorToTemplate(TemplateVendor templateVendor);
        void RemoveVendorFromTemplate(int templateVendorId);
        IEnumerable<TemplateVendor> GetVendorsForTemplate(int eventTemplateId);
    }
}
