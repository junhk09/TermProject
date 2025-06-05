using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshGameManager : MonoBehaviourPun // ������ ���� ���� ���� �� ���� UI�� �����ϴ� ���� �Ŵ��� ��ũ��Ʈ
{
    public static cshGameManager instance // �ܺο��� �̱��� ������Ʈ�� �����ö� ����� ������Ƽ
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<cshGameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static cshGameManager m_instance; // �̱����� �Ҵ�� static ����

    public GameObject PlayerPrefab; // ������ VR �÷��̾� ĳ����
    public GameObject SpawnPosPrefab; // ������ VR �÷��̾� ĳ������ ��ġ
    public GameObject BossPrefab;
    public GameObject BossSpawnPoint;


    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ������ ���� ��ġ ����
        Vector3 randomSpawnPos = SpawnPosPrefab.transform.position;//Random.insideUnitSphere * 5f;
        Vector3 randomSpawnPos1 = BossSpawnPoint.transform.position;
        
        PhotonNetwork.Instantiate(PlayerPrefab.name, randomSpawnPos, Quaternion.identity);
        PhotonNetwork.Instantiate(BossPrefab.name, randomSpawnPos1, Quaternion.identity);
    }
}
