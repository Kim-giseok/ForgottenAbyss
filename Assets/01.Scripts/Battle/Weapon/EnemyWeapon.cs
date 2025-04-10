using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class TestClass
{
    public void SayHello()
    {
        Debug.Log("Hello");
    }
}

public class TestClass2 : TestClass {}

public class EnemyWeapon: MonoBehaviour
{
    public List<GameObject> tests;
    public int skillId = 101;

    private void Start()
    {
        // SkillManager.Instance.TryUseSkill(skillId, transform);
        
        var baseType = typeof(TestClass);
        var derivedTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t != baseType && baseType.IsAssignableFrom(t));

        foreach (var type in derivedTypes)
        {
            TestClass instance = Activator.CreateInstance(type) as TestClass;
            instance.SayHello();
        }
    }
}