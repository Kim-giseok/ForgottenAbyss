using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoryPieceSO", menuName = "SO/MemoryPiece")]
public class MemoryPieceSO : ScriptableObject
{
    public int currentMemoryPieceId;
    public string displayName;
    public Sprite icon;
    public MemorySkillItem skillItem;
}