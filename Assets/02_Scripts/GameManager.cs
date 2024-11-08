using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button sendButton;

    [SerializeField] private TMP_Text connectionInfoText;
    [SerializeField] private TMP_Text messageListText;

    [SerializeField] private TMP_InputField messageIF;

    private void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
    }

    IEnumerator Start()
    {
        exitButton.onClick.AddListener(() => OnExitButtonClick());
        sendButton.onClick.AddListener(() => OnSendButtonClick());

        yield return new WaitForSeconds(0.2f);

        DisplayConnectInfo();
        CreateTank();

        yield return new WaitForSeconds(0.2f);
        PhotonNetwork.IsMessageQueueRunning = true;
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
        string msg = $"<color=#00ff00>[{newPlayer.NickName}]</color>님이 입장했습니다.";
        DisplayMessage(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayConnectInfo();
        string msg = $"<color=#ff0000>[{otherPlayer.NickName}]</color>님이 퇴장했습니다.";
        DisplayMessage(msg);
    }

    [PunRPC]
    private void DisplayMessage(string msg)
    {
        messageListText.text += $"{msg}\n";
    }
}
