using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SS13ServerApi.Services
{
    public class AccessCleanupService : IHostedService, IDisposable
    {
        private Timer _cleanupTimer;
        private AccessControlService CtrlService { get; }
        public AccessCleanupService(AccessControlService ctrlService) => this.CtrlService = ctrlService;

        private void OnTimerElapsed(object state)
        {
            var curTime = DateTime.Now;
            foreach (var kvp in CtrlService.Data)
            {
                if ((curTime - kvp.Value.CreatedTime).TotalSeconds >= kvp.Value.LifespanSeconds)
                {
                    CtrlService.Data.Remove(kvp.Key);
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cleanupTimer = new Timer(OnTimerElapsed, null, 0, (int)TimeSpan.FromMinutes(10).TotalMilliseconds);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cleanupTimer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() => _cleanupTimer.Dispose();
    }
}
