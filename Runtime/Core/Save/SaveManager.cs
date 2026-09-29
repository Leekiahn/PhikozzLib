using System;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Sirenix.Serialization;
using UnityEngine;

namespace PhikozzLib
{
    public class SaveManager : MonoBehaviour, ISaveService, IServiceRegister
    {
        [SerializeField] private eSaveType _saveType = eSaveType.Json;
        [SerializeField] private string _saveDirectory = "Save";

        public void RegisterService()
        {
            ServiceLocator.Register<ISaveService>(this);
        }

        public void UnregisterService()
        {
            ServiceLocator.Unregister<ISaveService>();
        }

        public void Save<T>(string key, T data)
        {
            string filePath = GetFilePath(key);

            try
            {
                CreateDirectoryForFile(filePath);
                File.WriteAllBytes(filePath, Serialize(data));
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to save data for key '{key}'.", e);
            }
        }

        public async UniTask SaveAsync<T>(string key, T data)
        {
            string filePath = GetFilePath(key);
            byte[] bytes = Serialize(data);

            await UniTask.RunOnThreadPool(() =>
            {
                CreateDirectoryForFile(filePath);
                File.WriteAllBytes(filePath, bytes);
            });
        }

        public bool Exists(string key)
        {
            return File.Exists(GetFilePath(key));
        }

        private byte[] Serialize<T>(T data)
        {
            switch (_saveType)
            {
                case eSaveType.Json:
                    return Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                case eSaveType.Binary:
                    return SerializationUtility.SerializeValue(data, DataFormat.Binary, new SerializationContext());
                default:
                    throw new NotSupportedException($"Save type '{_saveType}' is not supported.");
            }
        }

        public eSaveLoadResult Load<T>(string key, out T data)
        {
            string filePath = GetFilePath(key);

            if (!File.Exists(filePath))
            {
                data = default;
                return eSaveLoadResult.NotFound;
            }

            try
            {
                data = Deserialize<T>(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Save data for key '{key}' is corrupted.\n{e}");
                data = default;
                return eSaveLoadResult.Corrupted;
            }

            if (data == null)
            {
                Debug.LogError($"[SaveManager] Save data for key '{key}' is empty.");
                return eSaveLoadResult.Corrupted;
            }

            return eSaveLoadResult.Success;
        }

        private T Deserialize<T>(string filePath)
        {
            switch (_saveType)
            {
                case eSaveType.Json:
                    return JsonUtility.FromJson<T>(File.ReadAllText(filePath));
                case eSaveType.Binary:
                    return SerializationUtility.DeserializeValue<T>(File.ReadAllBytes(filePath), DataFormat.Binary, new DeserializationContext());
                default:
                    throw new NotSupportedException($"Save type '{_saveType}' is not supported.");
            }
        }
        
        public void Delete(string key)
        {
            File.Delete(GetFilePath(key));
        }

        public void DeleteFolder(string folder)
        {
            string folderPath = Path.Combine(GetSaveDirectoryPath(), folder);

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }

        public void DeleteAll()
        {
            string directoryPath = GetSaveDirectoryPath();

            foreach (string file in Directory.GetFiles(directoryPath))
            {
                File.Delete(file);
            }

            foreach (string folder in Directory.GetDirectories(directoryPath))
            {
                Directory.Delete(folder, true);
            }
        }

        private string GetSaveDirectoryPath()
        {
            string path = Path.Combine(Application.persistentDataPath, _saveDirectory);

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

        private void CreateDirectoryForFile(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        private string GetExtension()
        {
            switch (_saveType)
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