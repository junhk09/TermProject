using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;
public class Player : MonoBehaviourPun
{
    public float speed;
    public int HP = 100;
    public GameObject gameOverUI;
    public TextMeshProUGUI hpText;
    float hAxis;
    float vAxis;
    float fireDelay;
    Vector3 moveVec;
    Animator anim;
    bool wDown;
    Rigidbody rigid;
    bool jDown;
    bool isJump;
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
            hpText = GameObject.Find("PlayerHP").GetComponent<TextMeshProUGUI>();
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
        UpdateHpUI();
        if (HP <= 0)
        {
            Die();
        }
    }
    void UpdateHpUI()
    {
        if (photonView.IsMine && hpText != null)
        {
            hpText.text = "HP: " + HP.ToString();
        }
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
        if (collision.gameObject.CompareTag("Heart"))
        {
            Destroy(collision.gameObject);
            HP += 20;
            HP = Mathf.Min(100, HP);
            UpdateHpUI();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            photonView.RPC("ChangeColor", RpcTarget.All);
            HP -= 20;
        }
    }
   
    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine) return;
     
        HP -= damage;
        HP = Mathf.Max(0, HP);
        UpdateHpUI();
        Debug.Log($"[Player] 피해 입음: {damage}, 남은 HP: {HP}");
        photonView.RPC("ChangeColor", RpcTarget.All);
        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
    }
    [PunRPC]
    public void ChangeColor()
    {
        StartCoroutine(DamageColorFlash());
    }

    IEnumerator DamageColorFlash()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            r.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (Renderer r in renderers)
        {
            r.material.color = Color.white;
        }
    }
    void Die()
    {
        Debug.Log("[Player] 사망 처리");
        cshGameManager.instance.ShowGameOverUI();
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
