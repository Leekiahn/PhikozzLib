using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Object = UnityEngine.Object;


namespace PhikozzLib
{
    public class LocalizationManager : MonoBehaviour, ILocalizationService, IServiceRegister
    {
        [SerializeField] private LocalizationSettings _localizationSettings;

        public void RegisterService()
        {
            ServiceLocator.Register<ILocalizationService>(this);
        }

        public void SetLocale(string localeCode)
        {
            var locale = _localizationSettings.GetAvailableLocales().GetLocale(localeCode);
            _localizationSettings.SetSelectedLocale(locale);
        }

        public string GetString(string localeTableRef, string localeEntryRef)
        {
            LocalizedString localizedString = new LocalizedString { TableReference = localeTableRef, TableEntryReference = localeEntryRef };
            return localizedString.GetLocalizedString();
        }
        
        public string GetString(string localeTableRef, string localeEntryRef, LocalizedString.ChangeHandler onChanged, params object[] arguments)
        {
            LocalizedString localizedString = new LocalizedString
            {
                TableReference = localeTableRef,
                TableEntryReference = localeEntryRef,
                Arguments = arguments
            };
            
            localizedString.StringChanged += onChanged;
            localizedString.RefreshString();
            return localizedString.GetLocalizedString();
        }

        public async UniTask<T> GetAsset<T>(string localeTableRef, string localeEntryRef) where T : Object
        {
            LocalizedAsset<T> localizedAsset = new LocalizedAsset<T> { TableReference = localeTableRef, TableEntryReference = localeEntryRef };
            return await localizedAsset.LoadAssetAsync().ToUniTask();
        }
    }
}