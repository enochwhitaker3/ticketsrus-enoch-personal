using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualBasic;

namespace EnochTelemetry
{
    public static class EnochMetrics
    {
        public static readonly string MetricsName = "EnochMetric";
        public static Meter MeterOTickets = new Meter(MetricsName, "1.0.0");
        public static Counter<int> ticketCounter = MeterOTickets.CreateCounter<int>("Ticket", description: "Counts the number of tickets");

        static Meter meter = new Meter(MetricsName);
        public static Counter<int> counter = meter.CreateCounter<int>("Basic_Counter");
        public static UpDownCounter<int> up_n_down = meter.CreateUpDownCounter<int>("Up and Down Counter");

    }
}
