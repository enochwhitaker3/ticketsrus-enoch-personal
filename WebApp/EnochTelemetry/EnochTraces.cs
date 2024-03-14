using System.Diagnostics;

namespace WebApp.EnochTelemetry
{
    public static class EnochTelemetry
    {
        public static readonly string OccasionsSource = "GetAllOccasions";
        public static readonly string TicketsSource = "GetAllTickets";
        public static readonly ActivitySource EnochGetAllOccasions = new ActivitySource(OccasionsSource);
        public static readonly ActivitySource EnochGetAllTickets = new ActivitySource(TicketsSource);
    }
}
