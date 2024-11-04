﻿using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TraffiLearn.Infrastructure.BackgroundJobs;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.Persistence.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class BackgroundJobsExtensions
    {
        public static IServiceCollection AddBackgroundJobs(
            this IServiceCollection services)
        {
            var outboxSettings = services.BuildServiceProvider().GetOptions<OutboxSettings>();

            services.AddQuartz(configurator =>
            {
                var scheduler = Guid.NewGuid();

                configurator.SchedulerId = $"default-id-{scheduler}";
                configurator.SchedulerName = $"default-name-{scheduler}";

                ConfigureProcessOutboxMessagesJob(configurator, outboxSettings);
            });

            services.AddQuartzHostedService();

            return services;
        }

        private static void ConfigureProcessOutboxMessagesJob(
            IServiceCollectionQuartzConfigurator configurator, 
            OutboxSettings outboxSettings)
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configurator
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger => trigger.ForJob(jobKey)
                        .WithSimpleSchedule(
                            schedule => schedule
                                .WithIntervalInSeconds(
                                    outboxSettings.ProcessIntervalInSeconds)
                                .RepeatForever()));
        }
    }
}