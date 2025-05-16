using Cysharp.Threading.Tasks;
using UnityEngine;

public class IntroBattleScene: CutScene
{
    public ActorController playerActor;
    public ActorController npc;
    public ActorController nightBone;
    public ActorController archer;

    public GameObject npcOrigin;
    
    public FieldItemDropAnimator nightBoneItem;
    public RectTransform nightBoneItemComp;
    
    protected override void Init()
    {
        Actions = new[]
        {
            Do(async () =>
            {
                GameManager.Instance.PausePlayer();
                await UniTask.Delay(1000);
                
                GameManager.Instance.player.gameObject.SetActive(false);
                playerActor.transform.position = GameManager.Instance.player.transform.position;
                playerActor.gameObject.SetActive(true);
                
                // 초기 로드 시간 문제로 인해 중복 코드 발생
                CutSceneManager.Instance.SubCams.Init();
                SetCutSceneMode(true);
                SetSentence("여긴... 어디지? 아무것도 기억나지 않아.");
            }),
            Do(() =>
            {
                SetSentence("이름도, 이 곳에 온 이유도 아무것도 ...");
            }),
            Do(async () =>
            {
                SubCams.Focus(npc.transform);
                
                npc.Define(new SequenceNode(new Test1Node(true), new IdleNode(0)));
                nightBone.Define(new SequenceNode(new Test1Node(false), new ChargingNode(0)));
                await UniTask.WaitUntil(() => npc.IsEnd && nightBone.IsEnd);
                
                SetSentence("이런.. 젠장!", npc.transform);
                
                // SetSentence("또 한 명이 눈을 떴군. 너도... 그들 중 하나인가 보군", npc.transform);
                // await UniTask.Delay(3000);
                // SetSentence("돌아가고 싶다면, 우두머리들… 그들의 힘을 흡수해. 그리고 기억을 되찾아", npc.transform);
                // await UniTask.Delay(3000);
                // SetSentence("기억을 되찾는 건 곧 너의 정체를 마주하는 일이야. 준비는 되었나?", npc.transform);
            }),
            Do(async () =>
            { 
                SubCams.Focus(playerActor.transform);
                await UniTask.Delay(1000);
                
                playerActor.animHandler.Play("Dash");
                // BoltsPool.Instance.CreateParticle(playerActor.transform, "HolyAcua")
                //     .SetPosition(playerActor.transform.position).SetSize(1f).SetColor(new Color(255, 255, 255, 0.1f)).Play();
                
                playerActor.animHandler.SetSpeed(2);
                SoundManager.Instance.Playsfx("AgisSpell");
                
                await UniTask.Delay(400);
                
                playerActor.transform.position = nightBone.transform.position + Vector3.left * 1f;

                playerActor.animHandler.Play("Idle");
                playerActor.animHandler.SetSpeed(1);
                
                CutSceneManager.Instance.LetterBox.ShowNarration("A키를 눌러 콤보 공격이 가능합니다.");
                SetToolTip(new Vector3(0, 340, 0), "아무 키를 눌러 다음으로 진행해주세요.");
            }),
            Do(() => ComboAttackAction("SwordAttack_1")),
            Do(() => ComboAttackAction("SwordAttack_2")),
            Do(() => ComboAttackAction("SwordAttack_3")),
            Do(() => ComboAttackAction("SwordAttack_4")),
            Do(() => ComboAttackAction("SwordAttack_5")),
            Do(async () =>
            {
                playerActor.Rigidbody.drag = 10;
                playerActor.Rigidbody.AddForce(new Vector2(10f, 0), ForceMode2D.Impulse);

                ComboAttackAction("SwordAttack_6");
                await UniTask.Delay(400);

                playerActor.Rigidbody.velocity = Vector2.zero;
                playerActor.Rigidbody.drag = 0;

                
                CutSceneManager.Instance.LetterBox.HideNarration();
                ResetToolTip();
                nightBone.animHandler.Play("Die");
                await UniTask.Delay(800);

                SoundManager.Instance.Playsfx("DropItem");
                nightBoneItem.gameObject.SetActive(true);
                nightBoneItem.transform.position = nightBone.transform.position + Vector3.up * 1f;
                // nightBoneItem.Spawn();
                
                nightBone.gameObject.SetActive(false);

                SetToolTip(new Vector3(0, 340, 0), "아무 키를 눌러 다음으로 진행해주세요.");
                CutSceneManager.Instance.LetterBox.ShowNarration("메모리 스킬 아이템은 퀵슬롯에 등록하여 사용하실 수 있습니다.");
                SetPointingLight(nightBoneItem.transform);
            }),
            Do(() =>
            {
                CutSceneManager.Instance.LetterBox.HideNarration();

                ResetPointingLight();
                SoundManager.Instance.Playsfx("GetItem");
                Destroy(nightBoneItem.gameObject);
                
                UIManager.Instance.ShowIngameUI();
                UIManager.Instance.ToggleInventory();
                
                playerActor.animHandler.Play("Idle");
                
                nightBoneItemComp.SetParent(CutSceneManager.Instance.UIPool.transform);

                nightBoneItemComp.anchoredPosition = new Vector2(-230.9f, 127.26f);
                
                // 메서드로 한번 빼기
                CutSceneManager.Instance.UIPointingComp.gameObject.SetActive(true);
                CutSceneManager.Instance.UIPointingComp.anchoredPosition = new Vector2(-247.4f, 136.73f);

            }),
            Do(() =>
            {
                UIManager.Instance.ToggleInventory();
                nightBoneItemComp.anchoredPosition = new Vector2(-220.6f, -481.6f);
                CutSceneManager.Instance.UIPointingComp.anchoredPosition = new Vector2(-237f, -472.5f);
            }),
            Do(async () =>
            {
                ResetToolTip();
                UIManager.Instance.HideIngameUI();
                CutSceneManager.Instance.UIPointingComp.gameObject.SetActive(false);
                Destroy(nightBoneItemComp.gameObject);
                
                SoundManager.Instance.Playsfx("AgisSpell");
                playerActor.Rigidbody.AddForce(new Vector2(2, 1) * 4f, ForceMode2D.Impulse);
                playerActor.animHandler.Play("Dash");
                await UniTask.Delay(300);
                BoltsPool.Instance.CreateSummon(playerActor.transform, SummonSkillManager.Skill.DashAttack, true).Fire();
                await UniTask.Delay(400);

                BoltsPool.Instance.CreateParticle(transform, "Hit")
                    .SetSize(0.8f).SetColor(new Color(0, 0, 0, 0.2f)).SetPosition(archer.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
                BoltsPool.Instance.CreateParticle(transform, "Hit")
                    .SetSize(0.15f).SetColor(Color.yellow).SetPosition(archer.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
                SoundManager.Instance.Playsfx("HitByMelee");
                
                archer.animHandler.Play("Hit");
                await UniTask.Delay(250);
                playerActor.transform.position = archer.transform.position + Vector3.up * 0.15f;
                playerActor.animHandler.Play("Idle");
                Destroy(archer.gameObject);

                await UniTask.Delay(250);
                LightManager.Instance.FadeOut(1);
            }),
            
            Do(async () =>
            {   
                LightManager.Instance.FadeIn(1);
                await UniTask.Delay(500);
                
                // noticedestroy 방식으로하면 다시 재생이 안되므로 disable로 하기
                Destroy(npc.gameObject);
                npcOrigin.SetActive(true);
                playerActor.transform.position = npc.transform.position + Vector3.left;
                
                SetSentence("도와줘서 고맙네, 방금 사용한 그림자 능력은 무엇이지?", npcOrigin.transform);
                await UniTask.Delay(3000);
                SetSentence("기억이 나지 않아요. 여기는 어디인가요?", playerActor.transform);
            }),
            Do(() =>
            {
                ClearSentence();
                SetCutSceneMode(false);   
            })
        };
    }


    protected void ComboAttackAction(string animationName)
    {
        playerActor.animHandler.Play(animationName);
                
        BoltsPool.Instance.CreateParticle(transform, "Hit")
            .SetSize(0.8f).SetColor(new Color(0, 0, 0, 0.2f)).SetPosition(nightBone.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        BoltsPool.Instance.CreateParticle(transform, "Hit")
            .SetSize(0.15f).SetColor(Color.yellow).SetPosition(nightBone.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
        SoundManager.Instance.Playsfx("HitByMelee");
                
        nightBone.animHandler.Play("Hit");
    }
}