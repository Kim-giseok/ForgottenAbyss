using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapper : MonoBehaviour
{
    public RectTransform frontWeapon;
    public RectTransform backWeapon;

    public void SwapWeapons()
    {
        StartCoroutine(SwapAnimation());
    }

    private IEnumerator SwapAnimation()
    {
        float duration = 0.5f;
        float time = 0f;

        Vector3 frontStart = frontWeapon.anchoredPosition;
        Vector3 backStart = backWeapon.anchoredPosition;

        Vector3 frontTarget = backStart;
        Vector3 backTarget = frontStart;

        Vector3 frontScaleStart = frontWeapon.localScale;
        Vector3 backScaleStart = backWeapon.localScale;

        Vector3 frontScaleTarget = backScaleStart;
        Vector3 backScaleTarget = frontScaleStart;

        CanvasGroup frontCanvasGroup = frontWeapon.GetComponent<CanvasGroup>();
        CanvasGroup backCanvasGroup = backWeapon.GetComponent<CanvasGroup>();

        float frontAlphaStart = frontCanvasGroup.alpha;
        float backAlphaStart = backCanvasGroup.alpha;

        float frontAlphaTarget = 0.5f;
        float backAlphaTarget = 1f;

        while (time < duration)
        {
            float t = time / duration;

            frontWeapon.anchoredPosition = Vector3.Lerp(frontStart, frontTarget, t);
            backWeapon.anchoredPosition = Vector3.Lerp(backStart, backTarget, t);

            frontWeapon.localScale = Vector3.Lerp(frontScaleStart, frontScaleTarget, t);
            backWeapon.localScale = Vector3.Lerp(backScaleStart, backScaleTarget, t);

            frontCanvasGroup.alpha = Mathf.Lerp(frontAlphaStart, frontAlphaTarget, t);
            backCanvasGroup.alpha = Mathf.Lerp(backAlphaStart, backAlphaTarget, t);

            time += Time.deltaTime;
            yield return null;
        }

        // 마지막 위치 정리
        frontWeapon.anchoredPosition = frontTarget;
        backWeapon.anchoredPosition = backTarget;
        frontWeapon.localScale = frontScaleTarget;
        backWeapon.localScale = backScaleTarget;
        frontCanvasGroup.alpha = frontAlphaTarget;
        backCanvasGroup.alpha = backAlphaTarget;

        // 무기 참조 교체
        var temp = frontWeapon;
        frontWeapon = backWeapon;
        backWeapon = temp;
    }
}
