using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scrips.Util
{
    static class DiskOperations
    {
        public static void SaveText(string fileName, string contents)
        {
            var path = string.Format("{0}/{1}.json", Application.streamingAssetsPath, fileName);
            File.WriteAllText(path, contents);
        }

        public static string ReadText(string fileName)
        {
            var path = string.Format("{0}/{1}.json", Application.streamingAssetsPath, fileName);
            return File.ReadAllText(path);
        }

        public static List<string> GetModules()
        {
            var results = new List<string>();
            var filePaths = Directory.GetFiles(Application.streamingAssetsPath, "*Module.json", SearchOption.AllDirectories);
            foreach (var path in filePaths)
            {
                results.Add(File.ReadAllText(path));
            }
            return results;
        }
    }
}
