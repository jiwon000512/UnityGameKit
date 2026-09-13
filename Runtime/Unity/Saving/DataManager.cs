using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using GameKit.Singleton;

namespace GameKit.Saving
{
    public sealed class DataManager : MonoSingleton<DataManager>
    {
        public void Save<T>(string key, T data, int version)
        {
            var file = new SaveFile<T> { Version = version, SavedAtUtc = DateTime.UtcNow, Data = data };
            string path = GetPath(key);
            string tempPath = path + ".tmp";

            File.WriteAllText(tempPath, JsonConvert.SerializeObject(file));
            if (File.Exists(path))
            {
                File.Replace(tempPath, path, null);
            }
            else
            {
                File.Move(tempPath, path);
            }
        }

        public bool TryLoad<T>(string key, out SaveFile<T> file)
        {
            string path = GetPath(key);
            if (!File.Exists(path))
            {
                file = null;
                return false;
            }

            try
            {
                file = JsonConvert.DeserializeObject<SaveFile<T>>(File.ReadAllText(path));
            }
            catch (JsonException)
            {
                file = null;
            }

            return file != null;
        }

        public void Delete(string key)
        {
            File.Delete(GetPath(key));
        }

        private static string GetPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key + ".json");
        }
    }
}
