// using PhikozzLib;
// using UnityEngine;
//
// public static class ExamplePopupLoader
// {
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//     private static void LoadPopups()
//     {
//         var uiService = ServiceLocator.Get<IUIService>();
//
//         // BG_Popup: BGDatabase에서 만든 Popup 엔티티 (prefab 필드가 UIPopup 타입이라고 가정)
//         BG_Popup.ForEachEntity(popup =>
//         {
//             uiService.RegisterPopup(popup.prefab);
//         });
//     }
// }
