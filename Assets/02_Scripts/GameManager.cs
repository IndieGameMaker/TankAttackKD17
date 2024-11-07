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

    [SerializeField] private TMP_Text connectionInfoText;

    private void Awake()
    {
    }

    IEnumerator Start()
    {
        exitButton.onClick.AddListener(() => OnExitButtonClick());

        yield return new WaitForSeconds(0.5f);

        DisplayConnectInfo();
        CreateTank();
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

    private void OnExitButtonClick()
    {
        // 룸 Exit 요청
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
