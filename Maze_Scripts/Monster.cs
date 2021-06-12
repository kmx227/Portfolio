using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform Target;
    NavMeshAgent nav;
    public NavMeshSurface[] surfaces;
    Change map;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        map = FindObjectOfType<Change>();
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(Target.position);
    }

     
}
