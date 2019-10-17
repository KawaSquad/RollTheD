using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace KawaSquad
{
    namespace Network
    {
        public class SaveManager
        {
            [System.Serializable]
            public class JsonSave
            {
                public Dictionary<Guid, Pawn> pawns = new Dictionary<Guid, Pawn>();
            }
            public static JsonSave pawnsSave = new JsonSave();
            public static JsonSave pawnsLoad = new JsonSave();

            public static string jsonSave;

            public static string folderPath;
            public static string filePath;
            public static void Save()
            {
                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "ServerSave");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                filePath = Path.Combine(folderPath, "Save.json");
                if (File.Exists(filePath))
                    File.Delete(filePath);
                FileStream stream = File.Create(filePath);

                jsonSave = JsonConvert.SerializeObject(pawnsSave);
                byte[] buffer = Encoding.UTF8.GetBytes(jsonSave);
                stream.Write(buffer,0, buffer.Length);
                stream.Close();
            }
            public static void Load()
            {
                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "ServerSave");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                filePath = Path.Combine(folderPath, "Save.json");
                if (File.Exists(filePath))
                {
                    jsonSave = File.ReadAllText(filePath);
                    pawnsLoad = JsonConvert.DeserializeObject<JsonSave>(jsonSave);
                }
            }
        }
    }
}