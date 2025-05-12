using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class EnemyViewInfo
{
    public string enemyName;
    public float ratio; // x 와 y 사이즈 값
    public Vector2 size; // 콜라이더 관련 값
}

[CreateAssetMenu(menuName = "SO/Enemy/ViewInfoSO")]
public class EnemiesViewInfoSO: ScriptableObject
{
    public List<EnemyViewInfo> EnemyViewInfos;
}