using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GameKit.Singleton;

namespace GameKit.Tables
{
    public sealed class TableManager : MonoSingleton<TableManager>
    {
        private const string k_RootPath = "Data";

        private readonly Dictionary<string, object> m_cache = new Dictionary<string, object>();

        public IReadOnlyList<TRow> Load<TRow>(string tableName)
        {
            if (!m_cache.TryGetValue(tableName, out object cached))
            {
                cached = JsonConvert.DeserializeObject<TableFile<TRow>>(ReadText(tableName)).Rows;
                m_cache[tableName] = cached;
            }

            return (IReadOnlyList<TRow>)cached;
        }

        public T LoadConfig<T>(string tableName)
        {
            if (!m_cache.TryGetValue(tableName, out object cached))
            {
                cached = JsonConvert.DeserializeObject<T>(ReadText(tableName));
                m_cache[tableName] = cached;
            }

            return (T)cached;
        }

        private static string ReadText(string tableName)
        {
            return Resources.Load<TextAsset>($"{k_RootPath}/{tableName}").text;
        }
    }
}
