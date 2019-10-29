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
                public string currentMap = "";
                public string currentMapData = "";
            }
            public static JsonSave saveData = new JsonSave();

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
                {
                    string filePathOld = Path.Combine(folderPath, "Save_Old.json");

                    if (!File.Exists(filePathOld))
                    {
                        FileStream streamold = File.Create(filePathOld);
                        streamold.Close();
                    }
                    File.Replace(filePath, filePathOld, null);
                }
                FileStream stream = File.Create(filePath);

                jsonSave = JsonConvert.SerializeObject(saveData);
                byte[] buffer = Encoding.UTF8.GetBytes(jsonSave);
                stream.Write(buffer,0, buffer.Length);
                stream.Close();
            }
            public static void Load()
            {
                saveData = new JsonSave();

                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "ServerSave");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                filePath = Path.Combine(folderPath, "Save.json");
                if (File.Exists(filePath))
                {
                    try
                    {
                        jsonSave = File.ReadAllText(filePath);
                        saveData = JsonConvert.DeserializeObject<JsonSave>(jsonSave);
                    }
                    catch (Exception)
                    {
                        string filePathOld = Path.Combine(folderPath, "Save_Old.json");
                        if (File.Exists(filePathOld))
                        {
                            try
                            {
                                jsonSave = File.ReadAllText(filePathOld);
                                saveData = JsonConvert.DeserializeObject<JsonSave>(jsonSave);
                            }
                            catch (Exception)
                            {
                                Debug.LogError("Files corrupted",true);
                            }
                        }
                    }
                }
            }
        }
    }
}