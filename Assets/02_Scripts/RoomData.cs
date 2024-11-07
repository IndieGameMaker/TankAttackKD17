using Photon.Realtime;
using TMPro;
using UnityEngine;

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
        }
    }

    private void Awake()
    {
        roomText = GetComponentInChildren<TMP_Text>();
    }
}
