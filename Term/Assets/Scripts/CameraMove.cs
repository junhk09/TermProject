using Photon.Pun;
using UnityEngine;

public class CameraMove : MonoBehaviourPun
{
    public Transform target;
    public Vector3 offset;
    private void Update()
    {
        if (target == null) return;
        transform .position= target.position+offset;
    }
    
}
