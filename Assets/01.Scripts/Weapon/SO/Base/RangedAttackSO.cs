using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedAttackData", menuName = "SO/Combat/RangedAttackData")]
public class RangedAttackSO : ScriptableObject
{
    public int id;
    public string attackName;
    public string description;
    public Sprite icon;

    public string chargeStartAnim;
    public string chargeLoopAnim;
    public string chargeReleaseAnim;

    public float maxChargeTime;
    public float minPower;
    public float maxPower;
    public GameObject projectilePrefab;
}