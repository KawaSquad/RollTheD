using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSquad
{
    namespace Network
    {
        class General
        {
            public static void InitializeServer()
            {
                ServerTCP.InitailizeNetWork();
                Console.WriteLine("Server started");
            }
        }
    }
}