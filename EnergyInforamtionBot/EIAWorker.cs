// See https://aka.ms/new-console-template for more information

using Job.Scheduler.Job;
using Job.Scheduler.Job.Action;
using Job.Scheduler.Job.Exception;
using Microsoft.Extensions.Configuration;

namespace EnergyInforamtionBot
{
    /// <summary>
    /// ZThe EIA Worker implements an IRecuringJob interface and is used by the 
    /// scheduler to invoke the worker thread at the specified periodicity
    /// </summary>
    internal class EIAWorker : IRecurringJob
    {
        private readonly IConfiguration configuration;
        private EIAClient _client;
        private readonly EIADataContext _context;
        private readonly int _daysCount;

        /// <summary>
        /// Creates an EIAWorker
        /// </summary>
        /// <param name="configuration">Configuration injected by host</param>
        /// <param name="client">The client for communication with EIA</param>
        /// <param name="context">The database context</param>
        public EIAWorker(IConfiguration configuration, EIAClient client, EIADataContext context)
        {
            this.configuration = configuration;
            int? delay = configuration.GetValue<int>("executionDelay");
            Delay = TimeSpan.FromSeconds( delay ?? 1 );
            _client = client;
            _context = context;
            _daysCount = configuration.GetValue<int>("daysCount");
        }

        /// <summary>
        /// Specifies the delay between runs in seconds
        /// </summary>
        public TimeSpan Delay { get; private set; }

        /// <summary>
        /// Retry on fail 3 times
        /// </summary>
        public IRetryAction FailRule { get; } = new RetryNTimes(3);

        /// <summary>
        /// Thread should run for no more than 30 seconds
        /// </summary>
        public TimeSpan? MaxRuntime { get; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Collect and store the data
        /// </summary>
        /// <param name="cancellationToken">Thread cancelation token</param>
        /// <returns>Nothing</returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Running....");
            var list = await _client.GetSeries();
            var listToAdd = new List<EIASeriesItem>();
            var earliestDate = DateTime.Now - TimeSpan.FromDays(_daysCount);
            list.ForEach(item => {
                if (item.Period > earliestDate && !_context.SeriesItems.Any(i => item.Period == i.Period))
                {
                    listToAdd.Add(item);
                }
            });
            _context.SeriesItems.AddRange(listToAdd);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// called is a failure happens during a run
        /// </summary>
        /// <param name="exception">The exception that caused the failure</param>
        /// <returns>Nothing</returns>
        public Task OnFailure(JobException exception)
        {
            Console.WriteLine(exception.Message);
            return Task.CompletedTask;
        }



    }
}