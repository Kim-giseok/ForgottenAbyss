using Cysharp.Threading.Tasks;
using UnityEngine;

public class MoonstoneEndingCutScene: CutScene
{
    public EnemyController moonstone;
    public ActorController moonstoneActor;
    protected override async UniTask StartScene()
    {
        GameManager.Instance.PausePlayer();
        moonstone.gameObject.SetActive(false);
        UI.HideIngameUI();
        SetCutSceneMode(true);

        // await UniTask.Delay(100);
        // Time.timeScale = 0.5f;
        
        Player.transform.position = moonstone.transform.position + new Vector3(-2, 0, 0);
        Player.animator.Play("Act_Dash");
        Sound.Playsfx("AgisSpell");
        await UniTask.Delay(500);
        
        FadeScreen.SetFade(false, 0f);
        Sound.Playsfx($"Sword1");
        await UniTask.Delay(500);
        
        moonstone.Rigid.gravityScale = 0f;
        Player.controller.rigid.gravityScale = 0f;
        
        moonstone.Rigid.AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
        Player.controller.rigid.AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
        await UniTask.Delay(3000);
        FadeScreen.SetFade(true, 3f);


        await Narration("그림자가 그림자를 베었다.");
    }
}