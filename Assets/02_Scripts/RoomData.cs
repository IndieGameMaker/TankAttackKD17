using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [SerializeField] private TMP_Text roomText;

    private RoomInfo roomInfo;

    // 프로퍼티 선언 
    // Getter, Setter
    public RoomInfo RoomInfo
    {
        get
        {
            return roomInfo;
        }
        set
        {
            roomInfo = value;
            roomText.text = $"{roomInfo.Name} : {roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";
            // 버튼 이벤트 연결
            GetComponent<Button>().onClick.AddListener(() => PhotonNetwork.JoinRoom(roomInfo.Name));
        }
    }

    private void Awake()
    {
        roomText = GetComponentInChildren<TMP_Text>();
    }
}
