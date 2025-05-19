// using Cysharp.Threading.Tasks;
//
// public class FlashbackScene: CutScene
// {
//     protected override void Init()
//     {
//         Actions = new[]
//         {
//             Do(async () =>
//             {
//                 await UniTask.Delay(1000);
//                 
//                 UI.HideIngameUI();
//                 SetCutSceneMode(true);
//             }),
//             Do(() => {})
//         };
//     }
// }