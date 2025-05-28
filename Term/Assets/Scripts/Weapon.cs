using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public int damage;
    public float rate;
   
   public IEnumerator Shot()
    {
        GameObject instantBullet= Instantiate(bullet,bulletPos.position,bulletPos.rotation);
        Rigidbody bulletRigid= instantBullet.GetComponent<Rigidbody>();
        bulletRigid.linearVelocity = bulletPos.forward * 50;

        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();

        Vector3 caseVec=bulletCasePos.forward * Random.Range(-3,-2)+Vector3.up* Random.Range(2, 3);
        caseRigid.AddForce(caseVec,ForceMode.Impulse);
       caseRigid.AddTorque(Vector3.up*10,ForceMode.Impulse);
       yield return null;
    }


}
