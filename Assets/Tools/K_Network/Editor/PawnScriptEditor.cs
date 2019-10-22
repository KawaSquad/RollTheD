using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KawaSquad
{
    namespace Network
    {
        public class PawnScriptEditor : UnityEditor.AssetModificationProcessor
        {
            public static void OnWillCreateAsset(string path)
            {
                if (path.Contains(".asset") && path.Contains(".meta"))
                {
                    path = path.Replace(".meta", "");
                    string content = File.ReadAllText(path);
                    if (!content.Contains("MotherPawn") || !content.Contains("DaughterPawn"))
                        return;

                    string fileExt = Path.GetExtension(path);
                    string fileName = Path.GetFileNameWithoutExtension(path);
                    string newPath = path.Replace(fileExt, "") + ".cs";

                    string[] paths = AssetDatabase.FindAssets("TemplatePawn.cs");
                    if (paths.Length == 0 || paths.Length > 1)
                    {
                        Debug.LogError("Too many templates");
                        return;
                    }

                    string newContent = File.ReadAllText(paths[0]);
                    //File.Delete(path);
                    newContent = newContent.Replace("#CLASS_NAME#", fileName);

                    FileStream stream = File.Create(newPath);
                    byte[] buffer = System.Text.Encoding.ASCII.GetBytes(newContent);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();

                    AssetDatabase.Refresh();
                }
            }
        }

        [CreateAssetMenu(fileName = "NewPawn", menuName = "KawaSquad/Pawn")]
        public class MotherPawn : ScriptableObject
        {

        }
        [CreateAssetMenu(fileName = "NewTokenPawn", menuName = "KawaSquad/TokenPawn")]
        public class DaughterPawn : ScriptableObject
        {

        }
    }
}
