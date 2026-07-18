using System;
using System.Collections.Generic;
using System.Linq;
using PhikozzLib;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataManager : SerializedMonoBehaviour, IDataService, IServiceRegister
{
    public void RegisterService()
    {
        ServiceLocator.Register<IDataService>(this);

    }

//
//     public Dictionary<BaseData, TextAsset> Load()
//     {
//         var dataDictionary = new Dictionary<BaseData, TextAsset>();`
//     }
//
}