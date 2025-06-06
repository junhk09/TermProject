using UnityEngine;
using Photon.Pun;

public class BossMissile : Bullet
{
    public Transform target;
    public float speed = 20f;
    public float rotateSpeed = 5f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // ���� �̻��Ͽ� ����ȭ
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion toRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, toRotation, rotateSpeed * Time.fixedDeltaTime));

        rb.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ������ ó��
            Player player = other.GetComponent<Player>();
            if (player != null && player.photonView.IsMine)
            {
                player.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 50); // 50 ������
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Rock"))
        {
            Destroy(gameObject);
        }
    }
}
