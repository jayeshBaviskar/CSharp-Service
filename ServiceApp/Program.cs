using System;
using System.IO;
using System.Timers; // Thread Safe
using Topshelf;

namespace ServiceApp
{
    internal class Program
    {
        private readonly Timer timer;

        public Program()
        {
            timer = new Timer(1000) { AutoReset = true };
            timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] lines = new string[] { DateTime.Now.ToString() };
            File.AppendAllLines(@"D:\SteepData\dataset\new.txt", lines);
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<Program>(s =>
               {
                   s.ConstructUsing(service => new Program());
                   s.WhenStarted(service => service.Start());
                   s.WhenStopped(service => service.Stop());
               });

                x.RunAsLocalSystem();

                x.SetServiceName("Steep Data Upload");
                x.SetDescription("Service Push the Extracted Data on SQL Server");

            });


            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;




        }
    }
}