using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
    }

    private void CreateTank()
    {
        Vector3 pos = new Vector3(Random.Range(-100.0f, 100.0f),
                                  5.0f,
                                  Random.Range(-100.0f, 100.0f));

        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity);
    }
}
