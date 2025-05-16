using Cysharp.Threading.Tasks;
using UnityEngine;

public class IntroBattleScene: CutScene
{
    public Transform npc;
    
    protected override void Init()
    {
        Actions = new[]
        {
            Do(async () =>
            {
                GameManager.Instance.PausePlayer();
                await UniTask.Delay(1000);
                
                // 초기 로드 시간 문제로 인해 중복 코드 발생
                CutSceneManager.Instance.SubCams.Init();
                SetCutSceneMode(true);
                SetSentence("여긴... 어디지? 아무것도 기억나지 않아.");
            }),
            Do(() =>
            {
                SetSentence("이름도, 이 곳에 온 이유도.아무것도");
            }),
            Do(async () =>
            {
                SubCams.Focus(npc);
                
                SetSentence("또 한 명이 눈을 떴군. 너도... 그들 중 하나인가 보군", npc);
                await UniTask.Delay(3000);
                SetSentence("돌아가고 싶다면, 우두머리들… 그들의 힘을 흡수해. 그리고 기억을 되찾아", npc);
                await UniTask.Delay(3000);
                SetSentence("기억을 되찾는 건 곧 너의 정체를 마주하는 일이야. 준비는 되었나?", npc);
            }),
            Do(() =>
            {   
                SubCams.Reset();
                SetCutSceneMode(false);
                ClearSentence();
            })
        };
    }

}