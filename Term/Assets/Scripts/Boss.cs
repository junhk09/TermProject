using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
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

    Vector3 lookVec;
    bool isLook;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponentInChildren<Animator>();

        StartCoroutine(Think());
    }

    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (isLook)
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

        // 현재 접속 중인 모든 플레이어 탐색
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObj in allPlayers)
        {
            // 미사일 생성
            GameObject instantMissile = Instantiate(missile, transform.position, transform.rotation);
            BossMissile bossMissile = instantMissile.GetComponent<BossMissile>();
            bossMissile.target = playerObj.transform;

            yield return new WaitForSeconds(0.2f); // 약간의 간격을 줘도 됨
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
}
