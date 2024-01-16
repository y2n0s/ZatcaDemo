using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Jobs.configs
{
    public class InvoicesReportingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(InvoicesReportingBackgroundJob));
            options
                .AddJob<InvoicesReportingBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSchedule(CronScheduleBuilder
                            .CronSchedule("0 */01 * * * ?")
                            .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Egypt"))));
        }
    }
}
