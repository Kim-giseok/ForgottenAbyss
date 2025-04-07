using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skillDataList;

    private void Awake()
    {
        skillDataList = DataLoadUtil.LoadJsonData<SkillData>("Json/SkillData");
    }
}
