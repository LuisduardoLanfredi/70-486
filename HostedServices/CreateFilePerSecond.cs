using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace exam70486.HostedServices
{
    public class CreateFilePerSecond : IHostedService
    {
        private readonly ILogger<CreateFilePerSecond> _logger;
        private StreamWriter _writter;

        public CreateFilePerSecond(ILogger<CreateFilePerSecond> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _writter = File.AppendText("logfile.txt");

            var a = new Timer((obj) =>
            {
                _writter.WriteLine("log message");

            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _writter.Dispose();
            return Task.CompletedTask;
        }
    }
}
