using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    public float speed;
    public int HP = 100;
    public GameObject gameOverUI; // [↘️ Inspector에서 연결 필요]

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

    void Start()
    {
        if (photonView.IsMine)
        {
            GameObject boss = GameObject.FindWithTag("Enemy");
            if (boss != null)
            {
                boss.GetComponent<Boss>().target = this.transform;
            }

            // 카메라 타겟 설정
            Camera.main.GetComponent<CameraMove>().target = this.transform;
        }
        else
        {
            // GameOver UI는 본인만 보이게
            if (gameOverUI != null)
                gameOverUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

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

    void Attack()
    {
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady)
        {
            photonView.RPC("OnShot", RpcTarget.All);
            fireDelay = 0;
        }
    }

    [PunRPC]
    void OnShot()
    {
        anim.SetTrigger("doShot");
        StartCoroutine(equipWeapon.Shot());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine) return;

        HP -= damage;
        Debug.Log($"[Player] 피해 입음: {damage}, 남은 HP: {HP}");

        if (HP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("[Player] 사망 처리");

        if (photonView.IsMine)
        {
            cshGameManager.instance.ShowGameOverUI();
        }

        this.enabled = false;
    }
}
