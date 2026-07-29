using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


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

        public string GetLocalizedString(string localeTableRef, string localeEntryRef)
        {
            LocalizedString localizedString = new LocalizedString { TableReference = localeTableRef, TableEntryReference = localeEntryRef };
            return localizedString.GetLocalizedString();
        }

        public async UniTask<T> GetLocalizedAsset<T>(string localeTableRef, string localeEntryRef) where T : Object
        {
            LocalizedAsset<T> localizedAsset = new LocalizedAsset<T> { TableReference = localeTableRef, TableEntryReference = localeEntryRef };
            return await localizedAsset.LoadAssetAsync().ToUniTask();
        }
    }
}