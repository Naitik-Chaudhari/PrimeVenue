using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;

namespace PrimeVenue.Repository
{
    public class EventRequestRepository : IEventRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<EventRequest> GetAll()
        {
            return _context.EventRequests
                           .OrderBy(r => r.EventDate)
                           .Include(r => r.Customer)
                           .ToList();
        }

        public EventRequest GetById(int id)
        {
            return _context.EventRequests
                           .FirstOrDefault(r => r.Id == id);
        }

        public void Add(EventRequest request)
        {
            _context.EventRequests.Add(request);
            _context.SaveChanges();
        }

        public void Update(EventRequest request)
        {
            _context.EventRequests.Update(request);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.EventRequests.FirstOrDefault(r => r.Id == id);
            if (existing != null)
            {
                _context.EventRequests.Remove(existing);
                _context.SaveChanges();
            }
        }

        public IEnumerable<EventRequest> GetByCustomer(string customerId)
        {
            return _context.EventRequests
                           .Where(r => r.CustomerId == customerId)
                           .OrderByDescending(r => r.EventDate)
                           .ToList();
        }
    }
}
