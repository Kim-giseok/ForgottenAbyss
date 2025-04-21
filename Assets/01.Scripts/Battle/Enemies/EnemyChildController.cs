using UnityEngine;

// 현재 구조상 BT를 child 또한 가지는 것은 적절하지 않을 수 있다. 
public class EnemyChildController: EnemyController
{
    public EnemyController parentController;

    public float health; // 자체 체력을 가질 것인지?

    private void Awake()
    {
        base.Awake();
        parentController = GetComponentInParent<EnemyController>();
    }
    
    
}