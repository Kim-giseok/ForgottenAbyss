using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineReciever : Machine
{
    [Header("RecieveParameter")]
    [SerializeField] GameObject[] recieveItem;
    [SerializeField] int recieveNum;

    public void RecieveObject()
    {
        if (recieveItem.Length <= 0) return;
        for (int i = 0; i < recieveNum; i++)
            Instantiate(recieveItem[Random.Range(0,recieveItem.Length)], transform.position, Quaternion.identity).transform.parent = MapSpawnManager.Instance.SpawnedMap.transform;
    }
}
