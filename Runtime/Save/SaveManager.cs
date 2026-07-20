using System.IO;
using UnityEngine;
using System;
using System.Text;

namespace PhikozzLib
{
    public class SaveManager : MonoBehaviour, ISaveService, IServiceRegister
    {
        [SerializeField] private eSaveType saveType = eSaveType.Json;
        
        public void RegisterService()
        {
            ServiceLocator.Register<ISaveService>(this);
        }

        public void Save<T>(string key, T data)
        {
            string filePath = GetFilePath(key);
            string json = JsonUtility.ToJson(data);

            switch (saveType)
            {
                case eSaveType.Json:
                {
                    File.WriteAllText(filePath, json);
                    break;
                }
                case eSaveType.Binary:
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    File.WriteAllBytes(filePath, bytes);
                    break;
                }
            }
        }

        public T Load<T>(string key)
        {
            string filePath = GetFilePath(key);
            
            switch (saveType)
            {
                case eSaveType.Json:
                {
                    string json = File.ReadAllText(filePath);
                    return JsonUtility.FromJson<T>(json);
                }
                case eSaveType.Binary:
                {
                    byte[] bytes = File.ReadAllBytes(filePath);
                    string json = Encoding.UTF8.GetString(bytes);
                    return JsonUtility.FromJson<T>(json);
                }
                default:
                {
                    return default;
                }
            }
        }

        public void Delete(string key)
        {
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

        private string GetSaveDirectoryPath()
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
    
        private string GetFilePath(string key)
        {
            return Path.Combine(GetSaveDirectoryPath(), $"{key}.{GetExtension()}");
        }

        private string GetExtension()
        {
            switch (saveType)
            {
                case eSaveType.Json:
                    return "json";
                case eSaveType.Binary:
                    return "bin";
                default:
                    return "txt";
            }
        }
    }
}
