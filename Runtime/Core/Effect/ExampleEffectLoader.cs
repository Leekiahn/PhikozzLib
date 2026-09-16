// using PhikozzLib;
// using UnityEngine;
//
// public static class ExampleEffectLoader
// {
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//     private static void LoadEffects()
//     {
//         var effectService = ServiceLocator.Get<IEffectService>();
//
//         // BG_Effect: BGDatabase에서 만든 Effect 엔티티 (name = key, prefab 필드 가정)
//         BG_Effect.ForEachEntity(effect =>
//         {
//             effectService.RegisterEffect(effect.name, effect.prefab);
//         });
//     }
// }
