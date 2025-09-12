using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public interface IEventRequestRepository
    {
        IEnumerable<EventRequest> GetAll();
        EventRequest GetById(int id);
        void Add(EventRequest request);
        void Update(EventRequest request);
        void Delete(int id);
        IEnumerable<EventRequest> GetByCustomer(string customerId);
    }
}
