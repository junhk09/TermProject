using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage=50;
    public bool isMelee;
    public bool isRock;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isRock && collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }
      

    }

 
}
