using UnityEngine;

[CreateAssetMenu(menuName = "SO/weapon")]
public class WeaponSO: ScriptableObject
{
    public float power = 1;
    public float cooldown = 0.5f;

    public Sprite image;
}