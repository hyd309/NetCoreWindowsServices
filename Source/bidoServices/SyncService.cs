using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace bidoServices
{
    public class SyncService : IBaseService
    {
        private readonly Timer _timer;

        public SyncService()
        {
            _timer = new System.Timers.Timer(10000);
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            _timer.Interval = 2000;

            _timer.AutoReset = true;
            _timer.Enabled = false;
            //logger = loggerFactory.CreateLogger<SyncService>();
        }

        private void OnTimedEvent(object source,ElapsedEventArgs e)
        {
            Console.WriteLine(string.Format("1SyncService:{0:yyyy-MM-dd HH:mm:sss}", DateTime.Now));
            _timer.Enabled = false;
            try
            {
                //do some job;
                Console.WriteLine("程序同步中....");
                System.Threading.Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                //logger.LogError("SyncService Error {0}:", ex.Message);
            }
            Console.WriteLine(string.Format("2SyncService:{0:yyyy-MM-dd HH:mm:sss}", DateTime.Now));
            _timer.Enabled = true;
        }

        public void Start()
        {
            _timer.Start();
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Enabled = false;
        }
    }
}
