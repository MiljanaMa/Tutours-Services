using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class NewsletterBackgroundService : BackgroundService
    {
        IServiceProvider _serviceProvider;
        public NewsletterBackgroundService(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Stakeholders background service starting...");

            stoppingToken.Register(() =>
                Console.WriteLine("Stakeholders background service stopping..."));

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Doing work....");

                // This eShopOnContainers method is querying a database table
                // and publishing events into the Event Bus (RabbitMQ / ServiceBus)
                //CheckConfirmedGracePeriodOrders();

                try
                {


                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        INewsletterPreferenceService scopedProcessingService =
                            scope.ServiceProvider.GetRequiredService<INewsletterPreferenceService>();
                        Console.Write("Candidates: ");
                        List<NewsletterPreferenceDto> candidates = scopedProcessingService.CheckCandidatesForNewsletter();
                        foreach(var candidate in candidates)
                        {
                            Console.WriteLine("[" + candidate.UserID + "," + candidate.Frequency + "," + candidate.LastSent + "]");
                        }
                        if(candidates.Count > 0)
                            scopedProcessingService.SendNewsletter(candidates);
                        await Task.Delay(30000, stoppingToken);
                    }

                    //Console.Write("Candidates: ");
                    //Console.Write(_newsletterService.CheckCandidatesForNewsletter());
                    //await Task.Delay(30000, stoppingToken);
                }
                catch (TaskCanceledException exception)
                {
                    Console.WriteLine("Task Canceled");
                }
            }
        }
    }
}
