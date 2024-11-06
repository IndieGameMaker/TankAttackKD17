using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 게임 버전 1.0 , 1.1
    [SerializeField] private const string version = "1.0";
    // 유저명
    [SerializeField] private string nickName = "Zack";

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
    }
}
