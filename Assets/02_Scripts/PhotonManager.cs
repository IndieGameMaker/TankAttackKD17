using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 게임 버전 1.0 , 1.1
    [SerializeField] private const string version = "1.0";
    // 유저명
    private string nickName = "Zack";

    [Header("UI")]
    [SerializeField] private TMP_InputField nickNameIF;
    [SerializeField] private TMP_InputField roomNameIF;

    [Header("Button")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button makeRoomButton;

    private void Awake()
    {
        // 게임버전 설정
        PhotonNetwork.GameVersion = version;
        // 유저명 설정
        PhotonNetwork.NickName = nickName;
        // 방장이 씬을 로딩했을 때 다른 유저들에 자동으로 씬을 로딩 시켜주는 옵션
        PhotonNetwork.AutomaticallySyncScene = true;

        // 포톤 서버에 접속 요청
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속되었을 때 호출되는 콜백(Callback)
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");
        // Lobby 접속 요청
        PhotonNetwork.JoinLobby();
    }

    // 로비에 입장했을 때 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장 완료");
        // 랜덤한 방에 입장을 요청
        PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤 방입장 실패 했을 때 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"방입장 실패 : {returnCode} : {message}");

        // 룸 옵션 설정
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 20,
            IsOpen = true,
            IsVisible = true
        };
        // 룸 생성 요청
        PhotonNetwork.CreateRoom("MyRoom", ro);
    }

    // 방생성 완료 콜백
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
    }

    // 방 입장 완료 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log($"방 입장 완료 : {PhotonNetwork.CurrentRoom.Name}");

        if (PhotonNetwork.IsMasterClient)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("BattleField");
        }

        // PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);


        // 전투 씬으로 이동처리
    }
}
