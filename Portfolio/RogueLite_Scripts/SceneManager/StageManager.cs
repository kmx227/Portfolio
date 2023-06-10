using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Manager", menuName = "Scriptable Object/StageManager Data", order = 0)]

public class StageManager : ScriptableObject
{
    [SerializeField] private List<MapData> stage1;

    public List<MapData> Stage1 { get => stage1; set => stage1 = value; }
}
