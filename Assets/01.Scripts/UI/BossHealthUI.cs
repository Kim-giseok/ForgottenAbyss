using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum BossProfileType { Agis, MudEye }

[System.Serializable]
public class BossProfile {
    public BossProfileType profileType;
    public Sprite sprite;
}

public class BossHealthUI: MonoBehaviour
{
    public List<BossProfile> bossProfiles;
    private SpriteRenderer currentRenderer;
    public Image profileImage;

    public RectTransform gaugeTransform;
    public TextMeshProUGUI percentageText;
    
    const float maxWidth = 600f;
    
    public void Active(bool isActive) => gameObject.SetActive(isActive);

    public void SetProfile(BossProfileType bossProfileType)
    {
            var currProfile = bossProfiles.Find(profile => profile.profileType == bossProfileType);
            profileImage.sprite = currProfile.sprite;
            profileImage.color = Color.white;
    }

    public void ClearProfile()
    {
        profileImage.sprite = null;
        profileImage.color = Color.clear;
    }

    public void SetPercentage(int percentage)
    {
        var currPercentage = percentage < 0 ? 0 : percentage;
        percentageText.text = currPercentage + "%";

        float currWitdh = (currPercentage / 100f) * maxWidth;
        gaugeTransform.sizeDelta = new Vector2(currWitdh, gaugeTransform.sizeDelta.y);
    }
}