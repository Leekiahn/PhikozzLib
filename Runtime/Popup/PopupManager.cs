using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhikozzLib
{
    public class PopupManager : MonoBehaviour, IPopupService, IServiceRegister
    {
        private readonly Dictionary<Type, GameObject> _prefabByType = new();
        private readonly Dictionary<Type, GameObject> _openedPopupByType = new();

        private Transform _popupParent;

        public void RegisterService()
        {
            ServiceLocator.Register<IPopupService>(this);
        }

        public async UniTask Load(string label)
        {
            await Core.Addressable.Load<GameObject>(label);
            
            var prefabs = Core.Addressable.GetAll<GameObject>(label);
            
            foreach (var prefab in prefabs)
            {
                var popupType = prefab.GetComponent<UIPopup>().GetType();
                _prefabByType[popupType] = prefab;
                Debug.Log($"[{label}] 레이블의 팝업 프리팹 [{popupType}] 등록 완료");
            }

            _popupParent = transform;
        }

        public T Open<T>() where T : UIPopup
        {
            return null;
        }

        public void Close<T>() where T : UIPopup
        {
        }

        public void CloseAll()
        {
        }
    }
}