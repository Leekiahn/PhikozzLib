using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

namespace PhikozzLib
{
    public interface ILocalizationService
    {
        void SetLocale(string localeCode);
        string GetString(string localeTableRef, string localeEntryRef, params object[] arguments);
        // 사용 중지: 호출자가 구독을 해제할 수 없는 구조라 제거. (LocalizationManager 주석 참고)
        // string GetString(string localeTableRef, string localeEntryRef, LocalizedString.ChangeHandler onChanged, params object[] arguments);
        UniTask<T> GetAssetAsync<T>(string localeTableRef, string localeEntryRef) where T : Object;
    }
}