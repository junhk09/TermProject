using Photon.Pun;       // Photon �� ���� ����� ���� ���̺귯���� Unity���� ������Ʈ�� ��� ����
using Photon.Realtime;  // Realtime Network ���� ���� c# ���̺귯��
using UnityEngine.UI;
using TMPro;


public class cshLobbyManager : MonoBehaviourPunCallbacks // PUN �����Ҷ� override ����� �ڵ� �ۼ��ؾߵ�
{
    private string gameVersion = "1"; // ���� �������� ��Ī�ϱ� ���� string ��� ���ڻӸ� �ƴ� �ٸ� �͵� ��� ����

    public TextMeshProUGUI connectionInfoText;
    public Button joinButton;

    private void Start()
    {
        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = gameVersion;
        // ������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        // Room ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;
        // ���� �õ� ������ �ؽ�Ʈ�� ǥ��
        connectionInfoText.text = "Connecting to Master Server...";
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Boss Breaker";
        //base.OnConnectedToMaster();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "Offline: Not connected to Master Server\nRetrying connection...";

        PhotonNetwork.ConnectUsingSettings();
        //base.OnDisconnected(cause);
    }

    public void Connect()
    {
        // �ߺ� ���� �õ��� ���� ���� ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;

        // Master ������ ���� ���̶��
        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting to Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "Offline: Not connected to Master Server\nRetrying connection...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "No Rooms, Creating a new room...";

        // ���ο� ���� ����� (���� Name, ���� �ɼ� ����)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });

        //base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Successfully participated in the room";
        // ��� �� �����ڰ� Main ���� �ε��ϰ� ��
        // Unity ���� �����ϴ� SceneMangaer.LoadScene() �� ���� ���� ��� ���� ������Ʈ�� ���� �� ��Ʈ��ũ ���� ������ ���� ����
        PhotonNetwork.LoadLevel("Play");

        //base.OnJoinedRoom();
    }

}
