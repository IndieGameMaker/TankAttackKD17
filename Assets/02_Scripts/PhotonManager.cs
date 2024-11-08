using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance = null;

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

    [Header("Room List")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Transform contentTr;

    private void Awake()
    {
        Instance = this;

        // 버튼 비활성
        loginButton.interactable = false;
        makeRoomButton.interactable = false;

        // 게임버전 설정
        PhotonNetwork.GameVersion = version;
        // 유저명 설정
        PhotonNetwork.NickName = nickName;
        // 방장이 씬을 로딩했을 때 다른 유저들에 자동으로 씬을 로딩 시켜주는 옵션
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            // 포톤 서버에 접속 요청
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Start()
    {
        // 저장된 닉네임 로드
        nickName = PlayerPrefs.GetString("NICK_NAME", $"USER_{Random.Range(0, 1001):0000}");
        nickNameIF.text = nickName;

        // 버튼 이벤트 연결
        loginButton.onClick.AddListener(() => OnLoginButtonClick());
        makeRoomButton.onClick.AddListener(() => OnMakeRoomButtonClick());
    }

    public void SetNickName()
    {
        // 닉네임이 비여있는지 확인
        if (string.IsNullOrEmpty(nickNameIF.text))
        {
            nickName = $"USER_{Random.Range(0, 1001):0000}";
            nickNameIF.text = nickName;
        }
        else
        {
            nickName = nickNameIF.text;
        }

        PhotonNetwork.NickName = nickName;
        PlayerPrefs.SetString("NICK_NAME", nickName);
    }

    #region UI 콜백 함수
    private void OnLoginButtonClick()
    {
        SetNickName();

        PhotonNetwork.JoinRandomRoom();
    }

    private void OnMakeRoomButtonClick()
    {
        SetNickName();

        if (string.IsNullOrEmpty(roomNameIF.text))
        {
            roomNameIF.text = $"ROOM_{Random.Range(0, 10000)}";
        }

        var roomOptions = new RoomOptions
        {
            MaxPlayers = 20,
            IsOpen = true,
            IsVisible = true
        };
        // 룸 생성
        PhotonNetwork.CreateRoom(roomNameIF.text, roomOptions);
    }
    #endregion

    #region 포톤콜백함수

    // 룸 목록을 저장할 Dictionary 선언
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();

    // 룸 목록이 변경되면 호출되는 콜백
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            Debug.Log($"{room.Name} : {room.PlayerCount} / {room.MaxPlayers}");

            if (room.RemovedFromList == true) // 삭제된 룸
            {
                // 룸 삭제
                if (roomDict.TryGetValue(room.Name, out GameObject removedRoom))
                {
                    // 프리팹 인스턴스를 삭제
                    Destroy(removedRoom);
                    // 딕셔너리에서 키를 삭제
                    roomDict.Remove(room.Name);
                }
                continue;
            }

            // 새로 생성된 룸 , 변경된 경우 로직 처리
            // 처음 생성된 룸 (Dictionary 검색했을 때 결과가 없을 경우)
            if (roomDict.ContainsKey(room.Name) == false)
            {
                // Room 프리팹 생성
                var _room = Instantiate(roomPrefab, contentTr);
                // 룸 속성을 설정
                _room.GetComponent<RoomData>().RoomInfo = room;
                // 딕셔너리에 저장
                roomDict.Add(room.Name, _room);
            }
            else
            {
                // 룸이 존재하는 경우 룸 정보를 변경
            }
        }
    }

    // 포톤 서버에 접속되었을 때 호출되는 콜백(Callback)
    public override void OnConnectedToMaster()
    {
        loginButton.interactable = true;
        makeRoomButton.interactable = true;

        Debug.Log("서버 접속 완료");
        // Lobby 접속 요청
        PhotonNetwork.JoinLobby();
    }

    // 로비에 입장했을 때 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장 완료");
        // 랜덤한 방에 입장을 요청
        // PhotonNetwork.JoinRandomRoom();
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
    #endregion
}
