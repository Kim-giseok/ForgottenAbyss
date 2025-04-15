using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwapper : MonoBehaviour
{
    public RectTransform frontWeapon;
    public RectTransform backWeapon;

    void Start()
    {
        InitializeWeaponStates(); // 시작할때 무기 상태 초기화
    }

    private void InitializeWeaponStates()
    {
        frontWeapon.GetComponent<CanvasGroup>().alpha = 1f; // 투명도 조절
        backWeapon.GetComponent<CanvasGroup>().alpha = 0.7f;

        SetWeaponBrightness(frontWeapon, 1f); // 명도 조절
        SetWeaponBrightness(backWeapon, 0.7f);
    }

    public void SwapWeapons()
    {
        StartCoroutine(SwapAnimation());

        backWeapon.SetAsLastSibling(); // 앞으로 오는 무기를 맨 위로
        frontWeapon.SetAsFirstSibling(); // 뒤로 가는 무기를 맨 뒤로
    }

    void SetWeaponBrightness(RectTransform weapon, float brightness)
    {
        Image img = weapon.GetComponent<Image>();
        if (img != null)
        {
            img.color = new Color(brightness, brightness, brightness, img.color.a);
        }
    }

    private IEnumerator SwapAnimation()
    {
        float duration = 0.3f;
        float time = 0f;

        // 두 무기의 시작 위치와 목표 위치
        Vector3 frontStart = frontWeapon.anchoredPosition;
        Vector3 backStart = backWeapon.anchoredPosition;
        Vector3 frontTarget = backStart;
        Vector3 backTarget = frontStart;

        // 투명도 조절을 위한 CanvasGroup
        CanvasGroup frontCanvasGroup = frontWeapon.GetComponent<CanvasGroup>();
        CanvasGroup backCanvasGroup = backWeapon.GetComponent<CanvasGroup>();

        float frontAlphaStart = frontCanvasGroup.alpha;
        float backAlphaStart = backCanvasGroup.alpha;

        float frontAlphaTarget = 0.7f;
        float backAlphaTarget = 1f;

        while (time < duration)
        {
            float t = time / duration;

            frontWeapon.anchoredPosition = Vector3.Lerp(frontStart, frontTarget, t);
            backWeapon.anchoredPosition = Vector3.Lerp(backStart, backTarget, t);

            frontCanvasGroup.alpha = Mathf.Lerp(frontAlphaStart, frontAlphaTarget, t);
            backCanvasGroup.alpha = Mathf.Lerp(backAlphaStart, backAlphaTarget, t);

            time += Time.deltaTime;
            yield return null;
        }

        // 애니메이션이 끝나고 포지션, 투명값 정리
        frontWeapon.anchoredPosition = frontTarget;
        backWeapon.anchoredPosition = backTarget;
        frontCanvasGroup.alpha = frontAlphaTarget;
        backCanvasGroup.alpha = backAlphaTarget;

        // 무기 참조 교체
        var temp = frontWeapon;
        frontWeapon = backWeapon;
        backWeapon = temp;

        // 무기 UI 순서 재배치
        frontWeapon.SetAsLastSibling(); // 앞으로 간 무기
        backWeapon.SetAsFirstSibling(); // 뒤로 간 무기

        // 밝기 조절
        SetWeaponBrightness(frontWeapon, 1f);   // 앞으로 나온 무기 밝게
        SetWeaponBrightness(backWeapon, 0.7f);  // 뒤로 간 무기 어둡게
    }
}
