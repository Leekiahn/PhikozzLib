using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PhikozzLib
{
    public interface ILocalizationService
    {
        void SetLocale(string localeCode);
        string GetLocalizedString(string localizationTable, string entryKey);
        UniTask<T> GetLocalizedAsset<T>(string localeTableRef, string localeEntryRef) where T : Object;
    }
}