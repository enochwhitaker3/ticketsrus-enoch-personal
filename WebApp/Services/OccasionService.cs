using EnochTelemetry;
using Microsoft.EntityFrameworkCore;
using RazorClassLib.Data;
using RazorClassLib.Services;
using WebApp.EnochTelemetry;

namespace WebApp.Services
{
    public partial class OccasionService : IOccasionService
    {
        private readonly ILogger<OccasionService> logger;
        private IDbContextFactory<TicketContext> contextFactory;

        [LoggerMessage(Level = LogLevel.Information, Message = "Starting Logs for Getting All Events {description}")]
        static partial void LogGetAllEventsMessage(ILogger logger, string description);

        [LoggerMessage(Level = LogLevel.Information, Message = "Starting Logs for Getting Single Event by ID {description}")]
        static partial void LogGetSingleEventMessage(ILogger logger, string description);

        [LoggerMessage(Level = LogLevel.Critical, Message = "Starting Logs for Adding an Event {description}")]
        static partial void LogAddEvent(ILogger logger, string description);

        public OccasionService(ILogger<OccasionService> logger, IDbContextFactory<TicketContext> contextFactory)
        {
            this.logger = logger;
            this.contextFactory = contextFactory;
        }
        public async Task AddNewOccasion(Occasion occasion)
        {
            var context = contextFactory.CreateDbContext();
            context.Add(occasion);
            LogAddEvent(logger, $"Added Event {occasion.Id}"); //--------------------------------
            await context.SaveChangesAsync();
        }

        public Task DropTables()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Occasion>> GetAllOccasions()
        {
            using var myActivity = EnochTraces.EnochGetAllOccasions.StartActivity("Getting All Occasions");
            EnochMetrics.ticketCounter.Add(5);
            var context = contextFactory.CreateDbContext();
            LogGetAllEventsMessage(logger, "Got All Occasions");

            EnochMetrics.counter.Add(1);
            EnochMetrics.up_n_down.Add(-1);

            return await context.Occasions
                .Include(o => o.Tickets)
                .ToListAsync();
        }

        public async Task<Occasion> GetOccasion(int id)
        {
            LogGetSingleEventMessage(logger, $"Getting event {id}"); //------------------------------
            var context = contextFactory.CreateDbContext();
#pragma warning disable CS8603 // Possible null reference return.

            return await context.Occasions
                .Where(o => o.Id == id)
                .Include(o => o.Tickets)
                .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.

        }

        public async Task<Occasion> GetOccasionId(string name)
        {
            var context = contextFactory.CreateDbContext();
#pragma warning disable CS8603 // Possible null reference return.
            return await context.Occasions
                .Where(o => o.OccasionName == name)
                .Include(o => o.Tickets)
                .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
