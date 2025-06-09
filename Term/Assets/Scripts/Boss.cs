using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Boss : MonoBehaviourPun
{
    public int maxHP = 500;
    private int currentHP;
    public float moveSpeed = 3f;
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;
    public Animator anim;
    public Transform target;
    public GameObject bullet;
    public BoxCollider meleeArea;
    public bool isDead;
    [Header("Boss HP UI")]
    public Image hpBar;

   
 
    Vector3 lookVec;
    bool isLook;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponentInChildren<Animator>();

        currentHP = maxHP;
        GameObject hpBg = GameObject.Find("HPBackground");
        if (hpBg != null)
        {
            Transform hpFill = hpBg.transform.Find("HPFill");
            if (hpFill != null)
            {
                hpBar = hpFill.GetComponent<Image>();
            }
        }
        StartCoroutine(Think());
    }

    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (isLook && target != null)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        if (target != null && !isDead)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            rigid.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
            transform.LookAt(target); // 플레이어 방향 바라보기
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                StartCoroutine(MissileShot());
                break;
        }
    }

    IEnumerator MissileShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObj in allPlayers)
        {
            GameObject instantMissile = Instantiate(missile, transform.position, transform.rotation);
            BossMissile bossMissile = instantMissile.GetComponent<BossMissile>();
            bossMissile.target = playerObj.transform;

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                Debug.Log("Boss Damaged!");
                // 테스트: 누가 맞추든 데미지 반영
                photonView.RPC("TakeDamage", RpcTarget.AllBuffered, bullet.damage);
            }

            Destroy(other.gameObject);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;


        if (currentHP <= 0)
        {
            Die();
        }
        UpdateHpUI();
    }
    void UpdateHpUI()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)currentHP / maxHP;
        }
    }
    void Die()
    {
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("die");
        StopAllCoroutines();
        if (PhotonNetwork.IsMasterClient)
        {
            cshGameManager.instance.ShowGameOverUIForAll(); // 모든 플레이어에게 알림
        }
        Destroy(gameObject);
        
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
}
