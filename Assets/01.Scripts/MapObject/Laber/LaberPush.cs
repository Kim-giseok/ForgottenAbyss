using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberPush : LaberBase
{
    private void OnTriggerEnter(Collider other)
    {
        SwitchMachine();
    }
}
