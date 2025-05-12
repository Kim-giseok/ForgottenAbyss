using System;
using UnityEngine;

public class FieldGoldItem: MonoBehaviour
{
    public int amount;
    private FieldItemDropAnimator _dropAnimator;

    private void Awake()
    {
        _dropAnimator = GetComponent<FieldItemDropAnimator>();
    }

    public void Spawn()
    {
        _dropAnimator.Spawn();
    }
}