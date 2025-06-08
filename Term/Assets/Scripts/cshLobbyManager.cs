using Photon.Pun;       // Photon 의 여러 기능을 포함 라이브러리를 Unity에서 컴포넌트로 사용 가능
using Photon.Realtime;  // Realtime Network 게임 개발 c# 라이브러리
using UnityEngine.UI;
using TMPro;


public class cshLobbyManager : MonoBehaviourPunCallbacks // PUN 구현할때 override 사용해 코드 작성해야됨
{
    private string gameVersion = "1"; // 같은 버전끼리 매칭하기 위해 string 사용 숫자뿐만 아닌 다른 것도 사용 가능

    public TextMeshProUGUI connectionInfoText;
    public Button joinButton;

    private void Start()
    {
        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // Room 접속 버튼 잠시 비활성화
        joinButton.interactable = false;
        // 접속 시도 중임을 텍스트로 표시
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
        // 중복 접속 시도를 막기 위해 접속 버튼 잠시 비활성화
        joinButton.interactable = false;

        // Master 서버에 접속 중이라면
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

        // 새로운 방을 만들며 (방의 Name, 방의 옵션 설정)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });

        //base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Successfully participated in the room";
        // 모든 룸 참가자가 Main 씬을 로드하게 함
        // Unity 에서 제공하는 SceneMangaer.LoadScene() 은 이전 씬의 모든 게임 오브젝트를 삭제 및 네트워크 정보 유지가 되지 않음
        PhotonNetwork.LoadLevel("Play");

        //base.OnJoinedRoom();
    }

}
