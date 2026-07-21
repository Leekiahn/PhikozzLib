using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Sirenix.Serialization;
using UnityEngine;

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

            switch (saveType)
            {
                case eSaveType.Json:
                {
                    string json = JsonUtility.ToJson(data);

                    try
                    {
                        File.WriteAllText(filePath, json);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failed to save data as JSON.", e);
                    }

                    break;
                }
                case eSaveType.Binary:
                {
                    try
                    {
                        byte[] bytes = SerializationUtility.SerializeValue(
                            data,
                            DataFormat.Binary,
                            new SerializationContext());

                        File.WriteAllBytes(filePath, bytes);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failed to save data as Binary.", e);
                    }

                    break;
                }
            }
        }

        public async UniTask SaveAsync<T>(string key, T data)
        {
            string path = GetFilePath(key);

            switch (saveType)
            {
                case eSaveType.Json:
                {
                    await UniTask.RunOnThreadPool(async () =>
                    {
                        string json = JsonUtility.ToJson(data);
                        await File.WriteAllTextAsync(path, json);
                    });

                    break;
                }

                case eSaveType.Binary:
                {
                    await UniTask.RunOnThreadPool(async () =>
                    {
                        byte[] bytes = SerializationUtility.SerializeValue(
                            data,
                            DataFormat.Binary,
                            new SerializationContext());

                        await File.WriteAllBytesAsync(path, bytes);
                    });

                    break;
                }
            }
        }

        public bool TryLoad<T>(string key, out T data)
        {
            string filePath = GetFilePath(key);

            switch (saveType)
            {
                case eSaveType.Json:
                {
                    try
                    {
                        string json = File.ReadAllText(filePath);
                        data = JsonUtility.FromJson<T>(json);
                        return true;
                    }
                    catch
                    {
                        data = default;
                        return false;
                    }
                }
                case eSaveType.Binary:
                {
                    try
                    {
                        byte[] bytes = File.ReadAllBytes(filePath);
                        data = SerializationUtility.DeserializeValue<T>(
                            bytes,
                            DataFormat.Binary,
                            new DeserializationContext());

                        return true;
                    }
                    catch
                    {
                        data = default;
                        return false;
                    }
                }
                default:
                {
                    data = default;
                    return false;
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
            string path = Path.Combine(Application.persistentDataPath, "Save");

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