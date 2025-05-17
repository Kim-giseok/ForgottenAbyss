using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class IntroBattleScene: CutScene
{
    [FormerlySerializedAs("playerActor")] public ActorController player;
    public ActorController npc;
    public ActorController nightBone;
    public ActorController archer;

    public GameObject npcOrigin;
    
    [FormerlySerializedAs("nightBoneItem")] public FieldItemDropAnimator item;
    [FormerlySerializedAs("nightBoneItemComp")] public RectTransform itemUI;
    
    protected override void Init()
    {
        Actions = new[]
        {
            Do(async () =>
            {
                // 빌리지에서 바로 시작할 경우 문제 발생
                GameManager.Instance.PausePlayer();
                await UniTask.Delay(1000);
                
                Player.gameObject.SetActive(false);
                player.transform.position = Player.transform.position;
                player.gameObject.SetActive(true);
                SetToolTip(new Vector3(0, 340, 0), "아무 키를 눌러 다음으로 진행해주세요.");
   
                // 초기 로드 시간 문제로 인해 중복 코드 발생
                SubCams.Init();
                SubCams.Focus(player.transform);
                SetCutSceneMode(true);
                SetSentence("아무것도 기억나지 않아...");
            }),
            Do(() => { SetSentence("이름도, 이 곳에 온 이유도 아무것도 ..."); }),
            Do(async () =>
            {
                SubCams.SetNoise(1, 1f);
                SubCams.Focus(npc.transform);
                SubCams.Zoom(true, 0.4f, 4.4f);

                npc.Define(new SequenceNode(new Test1Node(true), new IdleNode(0)));
                nightBone.Define(new SequenceNode(new Test1Node(false), new ChargingNode(0)));
                await UniTask.WaitUntil(() => npc.IsEnd && nightBone.IsEnd);
                
                SetSentence("이런.. 젠장!", npc.transform);
            }),
            Do(async () =>
            { 
                SubCams.SetNoise(0);
                SubCams.Focus(player.transform);
                await UniTask.Delay(1000);
                
                player.Anim.SetSpeed(2);
                player.Anim.Play("Dash");
                player.Rigid.drag = 10;
                player.Rigid.AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
                Sound.Playsfx("AgisSpell");
                await UniTask.Delay(300);
                
                player.transform.position = nightBone.transform.position + Vector3.left * 1f;

                player.Anim.SetSpeed(1);
                player.Anim.Play("Idle");
                player.Rigid.velocity = Vector2.zero;
                
                Sound.PlayBGM("IntroBattle");

                SetCurrInputKey(KeyCode.A);
                SetNarration("기본 공격을 통해 6연타 콤보 공격이 가능합니다.");
                SetToolTip(new Vector3(0, 340, 0), "A 키를 눌러 다음으로 진행해주세요.");
            }),
            Do(() => ComboAttackAction("SwordAttack_1")),
            Do(() => ComboAttackAction("SwordAttack_2")),
            Do(() => ComboAttackAction("SwordAttack_3")),
            Do(() => ComboAttackAction("SwordAttack_4")),
            Do(() => ComboAttackAction("SwordAttack_5")),
            Do(async () =>
            {
                player.Rigid.AddForce(new Vector2(10f, 0), ForceMode2D.Impulse);

                ComboAttackAction("SwordAttack_6");
                await UniTask.Delay(400);

                player.Rigid.velocity = Vector2.zero;
                player.Rigid.drag = 0;


                SetNarration();
                ResetToolTip();
                nightBone.Anim.Play("Die");
                await UniTask.Delay(800);

                Sound.Playsfx("DropItem");
                item.gameObject.SetActive(true);
                item.transform.position = nightBone.transform.position + Vector3.up * 1f;
                
                nightBone.gameObject.SetActive(false);

                SetCurrInputKey(KeyCode.I);
                SetToolTip(new Vector3(0, 340, 0), "I키를 눌러 인벤토리를 열어주세요.");
                SetNarration("메모리 스킬 아이템은 퀵슬롯에 등록하여 사용하실 수 있습니다.");
                SetPointingLight(item.transform);
            }),
            Do(() =>
            {
                SetToolTip(new Vector3(0, 340, 0), "I키를 눌러 인벤토리를 닫아주세요.");
                SetNarration();

                ResetPointingLight();
                Sound.Playsfx("GetItem");
                Destroy(item.gameObject);
                
                UIManager.Instance.ShowIngameUI();
                // UIManager.Instance.ToggleInventory();
                
                player.Anim.Play("Idle");
                
                itemUI.SetParent(CutSceneManager.Instance.UIPool.transform);
                itemUI.anchoredPosition = new Vector2(-230.9f, 127.26f);
                
                // 메서드로 한번 빼기
                CutSceneManager.Instance.UIPointingComp.gameObject.SetActive(true);
                CutSceneManager.Instance.UIPointingComp.anchoredPosition = new Vector2(-247.4f, 136.73f);
            }),
            Do(() =>
            {
                archer.Anim.Play("Charging");
                SetToolTip(new Vector3(0, 340, 0), "1번 키를 눌러 메모리 스킬을 사용해주세요.");
                SetCurrInputKey(KeyCode.Alpha1);
                
                // UIManager.Instance.ToggleInventory();
                
                itemUI.anchoredPosition = new Vector2(-220.6f, -481.6f);
                CutSceneManager.Instance.UIPointingComp.anchoredPosition = new Vector2(-237f, -472.5f);
            }),
            Do(async () =>
            {
                SetCurrInputKey(KeyCode.None);

                ResetToolTip();
                UI.HideIngameUI();
                // 방식 정리하기
                CutSceneManager.Instance.UIPointingComp.gameObject.SetActive(false);
                
                Destroy(itemUI.gameObject);
                
                Sound.Playsfx("AgisSpell");
                player.Rigid.AddForce(new Vector2(2, 1) * 4f, ForceMode2D.Impulse);
                player.Anim.Play("Dash");
                await UniTask.Delay(300);
                BoltsPool.Instance.CreateSummon(player.transform, SummonSkillManager.Skill.DashAttack, true).Fire();
                await UniTask.Delay(400);

                // Hit 매소드로 한번 묶기
                BoltsPool.Particle(transform, "Hit").SetSize(0.8f).SetColor(new Color(0, 0, 0, 0.2f)).SetPosition(archer.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
                BoltsPool.Particle(transform, "Hit").SetSize(0.15f).SetColor(Color.yellow).SetPosition(archer.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
                Sound.Playsfx("HitByMelee");
                
                SubCams.Shake(2, 2, 0.2f);
                archer.Anim.Play("Hit");
                await UniTask.Delay(250);
                player.transform.position = archer.transform.position + Vector3.up * 0.15f;
                player.Anim.Play("Idle");
                Destroy(archer.gameObject);

                Sound.FadeOutBGM(3);
                await UniTask.Delay(250);
                Light.FadeOut(1);
                await UniTask.Delay(2000);
                SetNarration("그림자 속에서 희미한 장면을 떠올렸다.");
                await UniTask.Delay(1000);

                // CueMachine.Next();
            }),
            
            Do(async () =>
            {   
                // noticedestroy 방식으로하면 다시 재생이 안되므로 disable로 하기
                SetNarration();
                
                Destroy(npc.gameObject);
                npcOrigin.SetActive(true);
                player.transform.position = npc.transform.position + Vector3.left;
                SubCams.Focus(npcOrigin.transform);
                Light.FadeIn(1);
                await UniTask.Delay(500);
                
                SetSentence("고마워요. 그런데... 방금 그 그림자의 힘, 대체 어떤 거죠?", npcOrigin.transform);
                await UniTask.Delay(4000);
                
                SubCams.Focus(player.transform);
                SetSentence("떠오르지 않아요.. 단지, 갑자기 이 장면이 낯설지 않게 느껴졌어요.", player.transform);
                
                await UniTask.Delay(3000);
                SubCams.Focus(npcOrigin.transform);
                SetSentence("이 곳은 그림자의 침공으로부터 마지막으로 남은 거점이에요.", npcOrigin.transform);
                await UniTask.Delay(3000);
                SetSentence("어쩌면... 그림자 속에서, 잃어버린 기억도 찾을 수 있을지 몰라요.", npcOrigin.transform);
            }),
            Do(() =>
            {
                Player.transform.position = player.transform.position;
                Player.gameObject.SetActive(true);
                
                SubCams.Reset();
                ClearSentence();
                SetCutSceneMode(false);   
            })
        };
    }

    private void ComboAttackAction(string animationName)
    {
        SubCams.Shake(2, 2, 0.2f);
        player.Anim.Play(animationName);
                
        BoltsPool.Instance.Particle(transform, "Hit").SetSize(0.8f).SetColor(new Color(0, 0, 0, 0.2f)).SetPosition(nightBone.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        BoltsPool.Instance.Particle(transform, "Hit").SetSize(0.15f).SetColor(Color.yellow).SetPosition(nightBone.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        SoundManager.Instance.Playsfx("HitByMelee");
                
        nightBone.Anim.Play("Hit");
    }
}