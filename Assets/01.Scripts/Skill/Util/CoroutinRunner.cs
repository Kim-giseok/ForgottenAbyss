using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinRunner : MonoBehaviour
{
    public void RunCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
