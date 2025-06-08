using UnityEngine;
using Photon.Pun;

public class Player2 : MonoBehaviourPun
{

    public float speed;
    float hAxis;
    float vAxis;
    float fireDelay;
    Vector3 moveVec;
    Animator anim;
    bool wDown;
    Rigidbody rigid;
    bool jDown;
    bool isJump;
    bool iDown;
    bool fDown;
    public Weapon equipWeapon;
    bool isFireReady = true;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!photonView.IsMine)
            return;


        GetInput();
        Move();
        Turn();
        Jump();
        Attack();



    }
    private void FixedUpdate()
    {
        FreezeRotation();
    }
    void Attack()
    {
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        if (fDown && isFireReady)
        {
            anim.SetTrigger("doShot");
            StartCoroutine(equipWeapon.Shot());
            fireDelay = 0;
        }
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
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
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void Jump()
    {
        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
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
