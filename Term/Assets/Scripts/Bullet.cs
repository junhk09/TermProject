using UnityEngine;

public class Bullet : MonoBehaviour
{
  
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }
      
    }
}
