using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.VisualBasic;

namespace EnochTelemetry
{
    public static class EnochMetrics
    {
        public static readonly string TicketMetricsName = "TicketMetric";
        public static Meter MeterOTickets = new Meter(TicketMetricsName, "1.0.0");
        public static Counter<int> ticketCounter = MeterOTickets.CreateCounter<int>("Ticket", description: "Counts the number of tickets");
    }
}