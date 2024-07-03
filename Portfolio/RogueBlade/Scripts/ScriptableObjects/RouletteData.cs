using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Roulette", menuName = "Scriptable Object/RouletteData")]
public class RouletteData : ScriptableObject
{
    public enum RouletteType { Shield, Attack, Weapon}

    [Header("# Main Info")]
    public RouletteType type;
    public int ID;

    [Header("# Roulette Data")]
    public Sprite sprite;
    public string text;
    public float percent;
    public int coin;

    [Header("# Upgrade Data")]
    public float[] power;
    public List<WeaponData> weapons;
}
