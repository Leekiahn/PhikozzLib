using System.IO;
using UnityEngine;
using System;

namespace PhikozzLib
{
    public class SaveManager : MonoBehaviour, ISaveService, IServiceRegister
    {
        public void RegisterService()
        {
            ServiceLocator.Register<ISaveService>(this);
        }

        public void Save<T>(string key, T data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(GetFilePath(key), json);
        }

        public T Load<T>(string key)
        {
            string json = File.ReadAllText(GetFilePath(key));
            return JsonUtility.FromJson<T>(json);
        }

        public void Delete(string key)
        {
            string json = File.ReadAllText(GetFilePath(key));
            File.Delete(GetFilePath(key));
        }

        public void DeleteAll()
        {
            string directoryPath = GetSaveDirectoryPath();
        
            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        private static string GetSaveDirectoryPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string productName = Application.productName;
            string path = Path.Combine(appDataPath, productName, "Save");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    
        private static string GetFilePath(string key)
        {
            return Path.Combine(GetSaveDirectoryPath(), key);
        }
    }
}
