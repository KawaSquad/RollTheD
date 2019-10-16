using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KawaSquad
{
    namespace Network
    {
        class Program
        {
            private static Thread thread;
            static void Main(string[] args)
            {
                General.InitializeServer();
                //Console.ReadLine();

                thread = new Thread(BusyWorkThread);
                thread.IsBackground = false;
                thread.Start();
            }

            public static void BusyWorkThread()
            {
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}