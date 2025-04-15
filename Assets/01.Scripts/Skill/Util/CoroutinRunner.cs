using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinRunner : Singleton<CoroutinRunner>
{
    public void RunCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
