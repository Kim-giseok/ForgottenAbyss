using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnmeyMode { Defense, Hit, IgnoreSturn }

// 디폴트는 동일하게 가져가지만 몬스터마다 다르게
public class EnemyStatusHandler: MonoBehaviour
{
    private LinkedList<EnemyStatus> CurrStatus { get; set; } = new();
    
    private Dictionary<int, bool> Modes { get; set; } = new();
    public void SetMode(EnmeyMode enemyMode, bool value) => Modes[(int)enemyMode] = value;
    
    public enum HitType { Normal, Stun }
    public HitType hitType { get; private set; }
    // public void SetHitType(HitType newHitType) => newHitType = newHitType;
    
    public bool GetMode(EnmeyMode enemyMode) => Modes[(int)enemyMode];


    private void Awake()
    {
        foreach (EnmeyMode mode in Enum.GetValues(typeof(EnmeyMode)))
        {
            Modes[(int)mode] = false;
        }
    }
    

    // public bool isIgnoreHitAction = false; // 원거리 친구만 false
    // public Vector3 startPosition;
    // public bool isDefense {get; private set;}
    // public int stunAccumulation;
    // public bool isFainted = false; // notice: 기절 기능 - 난이도
    // public int bullete; // 총알의 갯수
    // public int faintedDuration = 3;
    // public bool isCombat = false; // 한번 전투 중이면 지속 체크
    // public float castingDegree;
    
    // 보드에서 관리하기
    public Vector2 castingDirection; // 스킬을 발사하는 방향

    private void Update()
    {
        
    }
}