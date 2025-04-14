using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingArrowEffect : MonoBehaviour
{
    public string poolKey = "BowSkill02";
    public float speed = 5f;  // 화살 이동 속도
    public float rotationSpeed = 10f;  // 회전 속도
    public float maxDistance = 10f;  // 최대 추적 거리
    public float stayDuration = 0.1f;  // 효과 유지 시간
    public float hitRange = 0.3f;             // 타겟에게 도달한 거리로 간주하는 범위
    public float searchRadius = 10f;         // 타겟 탐색 범위
    public float preHomingTime = 0.2f;

    private Transform target;  // 추적할 목표
    private Vector3 startPosition;  // 시작 위치
    private bool isMoving = false;  // 이동 중 여부
    private bool isManualTarget = false;

    private Coroutine moveCoroutine;

    private GameObject caster;
    private SkillData skillData;

    private TrailRenderer trailRenderer;
    private AutoReleaseEffect autoReleaseEffect;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.Clear(); // 이펙트 재사용시 꼬리 초기화
        }

        autoReleaseEffect = GetComponent<AutoReleaseEffect>();
    }

    // 이펙트를 초기화하고, 추적할 타겟을 설정하는 함수
    public void Initialize(Vector3 startPos, Vector3 direction, GameObject caster, SkillData skillData)
    {
        if (skillData == null)
        {
            Debug.LogError("skillData가 null입니다!");
            return;
        }

        this.startPosition = startPos;
        this.caster = caster;
        this.skillData = skillData;

        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(direction);

        target = FindNearestEnemy(searchRadius);

        isMoving = true;

        if (trailRenderer != null)
            trailRenderer.Clear();


        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveTowardsTarget());
    }

    // 타겟을 향해 이동하는 코루틴
    private IEnumerator MoveTowardsTarget()
    {
        float timer = 0f;
        Vector3 initialDirection = transform.forward;

        while (timer < preHomingTime)
        {
            transform.position += initialDirection * speed * Time.deltaTime;
            timer += Time.deltaTime;

            if (Vector3.Distance(transform.position, startPosition) > maxDistance)
            {
                autoReleaseEffect.Release();
                yield break;
            }

            yield return null;
        }

        while (isMoving)
        {
            if (!isManualTarget && target == null)
            {
                target = FindNearestEnemy(searchRadius);
            }

            if (target != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (distanceToTarget > maxDistance)
                {
                    target = null;
                    continue;
                }
                else if (distanceToTarget < hitRange)
                {
                    OnHitTarget();
                    yield break;
                }

                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position += transform.forward * speed * Time.deltaTime;

                if (Vector3.Distance(transform.position, startPosition) > maxDistance)
                {
                    autoReleaseEffect.Release();
                    yield break;
                }
            }

            yield return null;
        }
    }

    private Transform FindNearestEnemy(float searchRadius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius, LayerMask.GetMask("Enemy"));

        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }

    // 타겟에 도달했을 때의 처리 (충돌 처리 및 데미지)
    private void OnHitTarget()
    {
        if (target != null)
        {
            SkillExecutionSO skillExecutionSO = DataManager.Instance.GetSkillExecutionSO(skillData.Name + "_Execution");

            if (skillExecutionSO == null)
            {
                Debug.LogError($"SkillExecutionSO를 찾을 수 없습니다. 이름: {skillData.Name}");
                autoReleaseEffect.Release();
                return;
            }

            skillExecutionSO.ExecuteSkill(caster, target.gameObject, skillData);

            // 카메라 쉐이크
            CameraShake.Instance.Shake(0.1f, 0.2f);
        }
        StartCoroutine(DisableAfterDelay(stayDuration));
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        autoReleaseEffect.Release();
    }

    public void SetManualTarget(Transform manualTarget)
    {
        target = manualTarget;
        isManualTarget = true;
    }
}
