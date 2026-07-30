using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

namespace PhikozzLib
{
    public interface ILocalizationService
    {
        void SetLocale(string localeCode);
        string GetString(string localizationTable, string entryKey);
        string GetString(string localeTableRef, string localeEntryRef, LocalizedString.ChangeHandler onChanged, params object[] arguments);
        UniTask<T> GetAsset<T>(string localeTableRef, string localeEntryRef) where T : Object;
    }
}