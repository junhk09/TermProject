using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.AI;
public class BossMissile : Bullet
{
    public Transform target;
    public NavMeshAgent nav;
    void Awake()
    {
        
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nav.SetDestination(target.position);
    }
}
