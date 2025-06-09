using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshGameManager : MonoBehaviourPun // 점수와 게임 오버 여부 및 게임 UI를 관리하는 게임 매니저 스크립트
{
    public static cshGameManager instance // 외부에서 싱글톤 오브젝트를 가져올때 사용할 프로퍼티
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<cshGameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static cshGameManager m_instance; // 싱글톤이 할당될 static 변수

    public GameObject PlayerPrefab; // 생성할 VR 플레이어 캐릭터
    public GameObject SpawnPosPrefab; // 생성할 VR 플레이어 캐릭터의 위치
    public GameObject BossPrefab;
    public GameObject BossSpawnPoint;
    public GameObject gameOverUI;

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 생성할 랜덤 위치 지정
        Vector3 randomSpawnPos = SpawnPosPrefab.transform.position;//Random.insideUnitSphere * 5f;
      
        
        PhotonNetwork.Instantiate(PlayerPrefab.name, randomSpawnPos, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 bossSpawnPos = BossSpawnPoint.transform.position;
            PhotonNetwork.Instantiate(BossPrefab.name, bossSpawnPos, Quaternion.identity);
        }
      
    }
    public void ShowGameOverUIForAll()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("ShowGameOverUI", RpcTarget.All);
    }
    [PunRPC]
    public void ShowGameOverUI()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }
}
