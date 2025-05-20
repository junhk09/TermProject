using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    Vector3 moveVec;
    Animator anim;
    bool wDown;
    Rigidbody rigid;
    bool jDown;
    bool isJump;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid=GetComponent<Rigidbody>();
    }
    private void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();




    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("walk");
        jDown = Input.GetButtonDown("Jump");
    }
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        anim.SetBool("Isrun", moveVec != Vector3.zero);
        anim.SetBool("Iswalk", wDown);
    }
    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown&&!isJump)
        {
            rigid.AddForce(Vector3.up*10,ForceMode.Impulse);
            isJump = true;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }


}
