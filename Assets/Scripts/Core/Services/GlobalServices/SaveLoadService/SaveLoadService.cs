using Newtonsoft.Json;
using UnityEngine;

namespace Elements.Core.Services.GlobalServices
{
    public class SaveLoadService : ISaveLoadService
    {
        public void Save(string key, string value) =>
            PlayerPrefs.SetString(key, value);

        public void Save<T>(string key, T obj) =>
            PlayerPrefs.SetString(key, JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            }));

        public string Load(string key) =>
            PlayerPrefs.GetString(key);

        public T Load<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key)) return default;

            return (T)JsonConvert.DeserializeObject(PlayerPrefs.GetString(key), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public bool HasKey(string key) =>
            PlayerPrefs.HasKey(key);
    }
}