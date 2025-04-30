using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : SingletonLoadRemain<SystemManager>
{
    public DataManager dataManager;
    public SkillManager skillManager;
    public WeaponManager weaponManager;
    public EquipmentManager equipmentManager;

    public EffectPool effect;
    public ProjectilePool projectile;

    public ActionBufferUtil actionBufferUtil;
    public CoroutinRunner coroutinRunner;

    protected override void Init()
    {
        base.Init();

        dataManager = GetComponent<DataManager>();
        skillManager = GetComponent<SkillManager>();
        weaponManager = GetComponent<WeaponManager>();
        equipmentManager = GetComponent<EquipmentManager>();

        effect = GetComponent<EffectPool>();
        projectile = GetComponent<ProjectilePool>();

        actionBufferUtil = GetComponent<ActionBufferUtil>();
        coroutinRunner = GetComponent<CoroutinRunner>();
    }
}
