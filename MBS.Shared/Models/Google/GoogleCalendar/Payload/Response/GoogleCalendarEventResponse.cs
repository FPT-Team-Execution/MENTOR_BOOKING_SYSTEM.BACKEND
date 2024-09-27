namespace MBS.Shared.Models.Google.Payload.Response;

public class GoogleCalendarEventResponse
{
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime Updated { get; set; }
        public string TimeZone { get; set; }
        public string AccessRole { get; set; }
        public List<Reminder> DefaultReminders { get; set; }
        public string NextSyncToken { get; set; }
        public List<GoogleCalendarEvent> Items { get; set; }
        
        public class Reminder
        {
                public string Method { get; set; }
                public int Minutes { get; set; }
        }
}