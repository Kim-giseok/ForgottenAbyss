using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class IntroBattleScene: CutScene
{
    public ActorController npc;
    public ActorController nightBone;
    public ActorController archer;

    public GameObject npcOrigin;
    
    [FormerlySerializedAs("nightBoneItem")] public FieldItemDropAnimator item;
    [FormerlySerializedAs("nightBoneItemComp")] public RectTransform itemUI;

    protected override async UniTask StartScene()
    {
        // 빌리지에서 바로 시작할 경우 문제 발생
        await UniTask.Delay(1000);
        
        // [초기 설정]
        GameManager.Instance.PausePlayer();
        UI.HideIngameUI();
        Camera.Init();
        LetterBox.Set(true);
        
        // [인트로]
        ToolTip.Set("아무 키를 눌러 다음으로 진행해주세요.", new Vector3(0, 340, 0));
        await Text("아무것도 기억나지 않아...");
        await Text("이름도, 이 곳에 온 이유도 아무것도 ...");

        // [추적 씬]
        Sound.Playsfx("Running");
        Camera.Focus(npc.transform);
        Camera.Zoom(true, 0.4f, 4.4f);
        Camera.Noise(1, 1f);

        npc.Anim.Play("Run");
        npc.Rigid.velocity = new Vector2(-4, 0);
        
        nightBone.Anim.Play("Run");
        nightBone.Rigid.velocity = new Vector2(-4, 0);
        
        await UniTask.Delay(1800);
        
        // [추적 종료]
        npc.Rigid.velocity = Vector2.zero;
        npc.transform.rotation = Quaternion.Euler(0, 0, 0);
        npc.Anim.Play("Idle");

        nightBone.Rigid.velocity = Vector2.zero;
        nightBone.Anim.Play("Charging");
        Sound.Playsfx("ElectronicCast");
        
        await Text("이런.. 젠장!", npc.transform);

        // [shift 입력 대기]
        Camera.Noise(0);
        Camera.Focus(Player);

        ToolTip.Set("왼쪽 Shift를 눌러 이동해주세요.", new Vector3(0, 340, 0));
        Narration("대시를 통해서 빠른 이동 및 적의 공격을 회피할 수 있습니다.").Forget();
        
        SetInput(KeyCode.LeftShift);
        await Wait();            
        
        // [대상을 향해 돌진]
        Player.animator.speed = 2;
        Player.animator.Play("Act_Dash");
        Player.controller.rigid.drag = 9;
        Player.controller.rigid.AddForce(new Vector2(20, 0), ForceMode2D.Impulse);
        Sound.Playsfx("AgisSpell");
        await UniTask.Delay(300);
        
        // [이동 완료]
        Player.animator.speed = 1;
        Player.animator.Play("Act_Idle");
        Player.controller.rigid.velocity = Vector2.zero;
        Player.transform.position = nightBone.transform.position + Vector3.left * 1f;
        
        // [공격 진행]
        Sound.PlayBGM("Intro1");
        ToolTip.Set("A 키를 눌러 다음으로 진행해주세요.", new Vector3(0, 340, 0));
        Narration("기본 공격을 통해 6연타 콤보 공격이 가능합니다.\n(본 게임에서는 플레이어 상단 콤보 게이지가 끊기지 않아야 합니다.)").Forget();
        
        SetInput(KeyCode.A);
        nightBone.Rigid.drag = 10;
        
        await ComboAttackAction("Act_SwordAttack_1");
        await ComboAttackAction("Act_SwordAttack_2");
        await ComboAttackAction("Act_SwordAttack_3");
        await ComboAttackAction("Act_SwordAttack_4");
        await ComboAttackAction("Act_SwordAttack_5");
        await ComboAttackAction("Act_SwordAttack_6");
        
        // [첫번째 전투 완료]
        nightBone.Rigid.AddForce(new Vector2(13, 0), ForceMode2D.Impulse);
        Player.controller.rigid.velocity = Vector2.zero;
        Player.controller.rigid.drag = 0;
        ToolTip.Set();
        Narration().Forget();
        nightBone.Anim.Play("Die");
        await UniTask.Delay(800);

        // [아이템 획득하기]
        Sound.Playsfx("DropItem");
        item.gameObject.SetActive(true);
        item.transform.position = nightBone.transform.position + Vector3.up * 1f;
        nightBone.gameObject.SetActive(false);
        
        ToolTip.Set("I키를 눌러 인벤토리를 열어주세요.", new Vector3(0, 340, 0));
        Narration("메모리 스킬 아이템은 퀵슬롯에 등록하여 사용하실 수 있습니다.").Forget();
        SetPointingLight(item.transform);
        
        SetInput(KeyCode.I);
        await Wait();  
        
        // [아이템 장착하기]
        Destroy(item.gameObject);
        Narration().Forget(); // Narration.Set 형태로 적용하기
        SetPointingLight(); // Set으로 통일하기
        ToolTip.Set("I키를 눌러 인벤토리를 닫아주세요.", new Vector3(0, 340, 0));
        Sound.Playsfx("GetItem");
        
        UI.ShowIngameUI();
        LetterBox.Set(false);
        UIPool.Set(itemUI,  new Vector2(-230.9f, 127.26f));
        // 메서드로 한번 빼기
        CutSceneManager.Instance.UIPointingComp.gameObject.SetActive(true);
        CutSceneManager.Instance.UIPointingComp.anchoredPosition = new Vector2(-247.4f, 136.73f);
        await Wait();  

        // [메모리 스킬 공격 대기]
        archer.Anim.Play("Charging");
        
        ToolTip.Set("1번 키를 눌러 메모리 스킬을 사용해주세요.", new Vector3(0, 340, 0));
        UIPool.Set(itemUI,new Vector2(-220.6f, -481.6f));
        CutSceneManager.Instance.UIPointingComp.anchoredPosition = new Vector2(-237f, -472.5f);
        
        SetInput(KeyCode.Alpha1); // Input.Set으로 변경하기
        await Wait();
        
        // [메모리 스킬 공격 진행]
        SetInput(KeyCode.None);
        ToolTip.Set();
        UI.HideIngameUI();
        LetterBox.Set(true);
        // 방식 정리하기
        CutSceneManager.Instance.UIPointingComp.gameObject.SetActive(false);
        Destroy(itemUI.gameObject);
        
        Player.controller.playerCollider.isTrigger = true;
        Player.controller.rigid.gravityScale = 0;
        
        Sound.Playsfx("AgisSpell");
        Player.controller.rigid.AddForce(new Vector2(2, 1) * 4f, ForceMode2D.Impulse);
        Player.animator.Play("Dash");
        await UniTask.Delay(300);
        Projectile.CreateSummon(Player.transform, SummonSkillManager.Skill.DashAttack, true).Fire();
        await UniTask.Delay(400);

        // Hit 매소드로 한번 묶기
        Projectile.Particle(transform, "Hit").SetSize(0.8f).SetColor(new Color(0, 0, 0, 0.2f)).SetPosition(archer.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        Projectile.Particle(transform, "Hit").SetSize(0.15f).SetColor(Color.yellow).SetPosition(archer.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        Sound.Playsfx("HitByMelee");
        
        Camera.Shake(2, 2, 0.2f);
        archer.Anim.Play("Hit");
        await UniTask.Delay(250);
        
        Player.transform.position = archer.transform.position + Vector3.up * 0.15f;
        Player.animator.Play("Idle");
        Destroy(archer.gameObject);

        // [나레이션]
        Sound.FadeOutBGM(3);
        await UniTask.Delay(250);
        Light.FadeOut(1);
        await UniTask.Delay(2000);
        await Narration("그림자 속에서 희미한 장면을 떠올렸다.");

        // [스토리 관련 대사 진행]
        Narration().Forget();
        // notice: destroy 방식으로하면 다시 재생이 안되므로 disable로 하기
        Destroy(npc.gameObject);
        npcOrigin.SetActive(true);
        Player.transform.position = npcOrigin.transform.position + Vector3.left * 3f;
        Camera.Focus(npcOrigin.transform);
        Light.FadeIn(1);
        await UniTask.Delay(500);
        
        await Text("고마워요. 그런데... 방금 그 그림자의 힘, 대체 어떤 거죠?", npcOrigin.transform);
        
        Camera.Focus(Player.transform);
        await Text("떠오르지 않아요.. 단지, 갑자기 이 장면이 낯설지 않게 느껴졌어요.", Player.transform);
        
        Camera.Focus(npcOrigin.transform);
        await Text("이 곳은 그림자의 침공으로부터 마지막으로 남은 거점이에요.", npcOrigin.transform);
        await Text("어쩌면... 그림자 속에서, 잃어버린 기억도 찾을 수 있을지 몰라요.", npcOrigin.transform);
        
        // [초기화] (리셋의 형태로 만들기)
        Player.transform.position = Player.transform.position;
        Player.gameObject.SetActive(true);
        
        Camera.Reset();
        await Text();
        SetCutSceneMode(false);
        GameManager.Instance.PausePlayer(false);
        Sound.Reset();
        
        Player.controller.playerCollider.isTrigger = false;
        Player.controller.rigid.gravityScale = 2;
        
        gameObject.SetActive(false);
    }

    private async UniTask ComboAttackAction(string animationName)
    {
        await Wait();

        Player.transform.position = nightBone.transform.position + Vector3.left * 1f;
        Camera.Shake(2, 2, 0.2f);
        Player.animator.Play(animationName);
                
        BoltsPool.Instance.Particle(transform, "Hit").SetSize(0.8f).SetColor(new Color(0, 0, 0, 0.2f)).SetPosition(nightBone.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        BoltsPool.Instance.Particle(transform, "Hit").SetSize(0.15f).SetColor(Color.yellow).SetPosition(nightBone.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        SoundManager.Instance.Playsfx("HitByMelee");
                
        nightBone.Anim.Play("Hit");
        nightBone.Rigid.AddForce(new Vector2(2, 0) * 2f, ForceMode2D.Impulse);
    }
}