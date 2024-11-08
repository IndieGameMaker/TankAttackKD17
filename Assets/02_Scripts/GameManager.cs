using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance = null;

    [Header("UI")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button sendButton;

    [SerializeField] private TMP_Text connectionInfoText;
    [SerializeField] private TMP_Text messageListText;
    [SerializeField] private TMP_Text playerListText;

    [SerializeField] private TMP_InputField messageIF;

    [SerializeField] PhotonManager.Map map;
    [SerializeField] PhotonManager.Difficulty difficulty;

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        exitButton.onClick.AddListener(() => OnExitButtonClick());
        sendButton.onClick.AddListener(() => OnSendButtonClick());
        messageIF.onEndEdit.AddListener((inputMessage) =>
        {
            string msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color> {inputMessage}";
            SendMessageByRPC(msg);
        }
        );

        yield return new WaitForSeconds(0.2f);

        CreateTank();
        DisplayConnectInfo();
        DisplayPlayerList();

        // 커스텀 프로퍼티 로드
        var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        map = (PhotonManager.Map)roomProperties["map"];
        difficulty = (PhotonManager.Difficulty)roomProperties["difficulty"];
    }

    private void CreateTank()
    {
        Vector3 pos = new Vector3(Random.Range(-100.0f, 100.0f),
                                  5.0f,
                                  Random.Range(-100.0f, 100.0f));

        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity);
    }

    private void DisplayConnectInfo()
    {
        int currentPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        string roomName = PhotonNetwork.CurrentRoom.Name;

        string connectStr = $"{roomName} (<color=#00ff00>{currentPlayer}</color>/<color=#ff0000>{maxPlayers}</color>)";
        connectionInfoText.text = connectStr;
    }

    private void DisplayPlayerList()
    {
        string playerList = "";

        foreach (var player in PhotonNetwork.PlayerList)
        {
            string _color = player.IsMasterClient ? "#ff0000" : "#00ff00";
            playerList += $"<color={_color}>{player.NickName}</color>\n";
        }

        playerListText.text = playerList;
    }

    public void SendMessageByRPC(string msg)
    {
        photonView.RPC(nameof(DisplayMessage), RpcTarget.AllBufferedViaServer, msg);
    }

    private void OnSendButtonClick()
    {
        string msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color> {messageIF.text}";
        SendMessageByRPC(msg);
    }


    private void OnExitButtonClick()
    {
        // 룸 Exit 요청
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DisplayConnectInfo();
        DisplayPlayerList();

        string msg = $"<color=#00ff00>[{newPlayer.NickName}]</color>님이 입장했습니다.";
        DisplayMessage(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayConnectInfo();
        DisplayPlayerList();

        string msg = $"<color=#ff0000>[{otherPlayer.NickName}]</color>님이 퇴장했습니다.";
        DisplayMessage(msg);
    }

    [PunRPC]
    private void DisplayMessage(string msg)
    {
        messageListText.text += $"{msg}\n";
    }
}
