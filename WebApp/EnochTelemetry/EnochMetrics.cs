using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualBasic;

namespace EnochTelemetry
{
    public static class EnochMetrics
    {
        public static int observableCountInteger = 0;
        public static int observableUpAndDownInteger = 0;
        public static readonly string MetricsName = "EnochMetric";
        public static Meter MeterOTickets = new Meter(MetricsName, "1.0.0");
        public static Counter<int> ticketCounter = MeterOTickets.CreateCounter<int>("Ticket", description: "Counts the number of tickets");

        static Meter meter = new Meter(MetricsName);
        public static Counter<int> counter = meter.CreateCounter<int>("Basic_Counter", description: "Counts the amount of refreshes on the home page");
        public static UpDownCounter<int> up_n_down = meter.CreateUpDownCounter<int>("Up_and_Down_Counter", description: "A counter that goes down by 1 everytime the home page refreshed");
        public static ObservableCounter<int> observableCounter = meter.CreateObservableCounter<int>("Observable_Counter", () => observableCountInteger, description: "Tracks an integer that goes up by one everytime someone visits the purchase ticket page");
        public static ObservableUpDownCounter<int> observableUpDownCounter = meter.CreateObservableUpDownCounter<int>("Observable_Up_N_Down", () => observableUpAndDownInteger, description: "Tracks an integer that goes up or down randomly when someone visits the purchase page");
        public static ObservableGauge<int> observableGauge = meter.CreateObservableGauge<int>("Observable_Gauge", () => System.DateTime.Now.Second);
        public static Histogram<int> histogram = meter.CreateHistogram<int>("Histogram", description: "A histogram showing how long it took to get all tickets");

    }
}
