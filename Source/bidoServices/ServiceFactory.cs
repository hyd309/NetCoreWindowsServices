using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace bidoServices
{
    public class ServiceFactory : IMicroService
    {
        private Timer timer = new Timer(1000);
        IMicroServiceController _contronller;
        IBaseService baseService;

        public ServiceFactory(IMicroServiceController controller, IEnumerable<IBaseService> enumerable)
        {
            _contronller = controller;
            foreach (var item in enumerable)
            {
                baseService=item;
            }
        }
        public ServiceFactory(IBaseService service)
        {
            baseService = service;
        }

        public void Start()
        {
            Console.WriteLine("I started");

            baseService.Start();
            /**
             * A timer is a simple example. But this could easily 
             * be a port or messaging queue client
             */
            //timer.Elapsed += _timer_Elapsed;
            //timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            //if (_contronller != null)
            //{
            //    _contronller.Stop();
            //}
            Console.WriteLine("_timer_Elapsed");
            baseService.Start();
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Stop();
            Console.WriteLine("I stopped");
        }
    }
}