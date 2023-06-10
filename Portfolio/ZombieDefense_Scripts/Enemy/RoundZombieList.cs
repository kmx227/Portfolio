using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundZombieList : MonoBehaviour
{
    [SerializeField] List<GameObject> _roundZombieList = new List<GameObject>();

    public List<GameObject> ZombieRoundList { get => _roundZombieList; set => _roundZombieList = value; }
}
