using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTrap : MachineLoop
{
    [SerializeField] int atk;
    [SerializeField] LayerMask attackLayer;
}
