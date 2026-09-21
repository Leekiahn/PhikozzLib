// using PhikozzLib;
// using UnityEngine;
//
// public static class ExampleAudioLoader
// {
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//     private static void LoadAudio()
//     {
//         var audioService = ServiceLocator.Get<IAudioService>();
//
//         // BG_Audio: BGDatabase에서 만든 Audio 엔티티 (name = key, clip 필드 가정)
//         BG_Audio.ForEachEntity(audio =>
//         {
//             audioService.RegisterAudio(audio.name, audio.clip);
//         });
//     }
// }
