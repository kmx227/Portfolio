using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/WeaponData")]
public class WeaponData : ScriptableObject
{
    public enum Weapon { Sword, Scythe, Hammer, Bow, Rifle }

    [Header("# Main Info")]
    public Weapon weapon;
    public int ID;
    public string Name;
    public Sprite sprite;

    [Header("# Attack Data")]
    public float power;
    public int count;
    public float range;
    public bool isCollison;
}
