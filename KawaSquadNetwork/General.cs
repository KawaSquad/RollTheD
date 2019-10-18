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
        class General
        {
            public static void InitializeServer()
            {
                ServerTCP.InitailizeNetWork();
                Debug.Log("Server started",true);

                //Load save
                SaveManager.Load();
                ClientManager.pawns = SaveManager.saveData.pawns;
                Thread threadAutoSave = new Thread(new ThreadStart(AutoSave));
                threadAutoSave.Start();
            }

            public static void AutoSave()
            {
                int timeSleep = 10 * 1000;//Sec
                while (true)
                {
                    Thread.Sleep(timeSleep);
                    SaveManager.saveData.pawns = ClientManager.pawns;
                    SaveManager.Save();
                }
            }
        }
    }
}