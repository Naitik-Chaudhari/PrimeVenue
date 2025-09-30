namespace PrimeVenue.Model.ViewModel
{
    public class OrganizerDashboardViewModel
    {
        public List<EventRequest> PendingRequests { get; set; }
        public List<EventRequest> TemplateSentRequests { get; set; }
        public List<EventRequest> FinalizedTemplateRequests { get; set; }
    }
}
