using Photon.Pun;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    private Transform tr;
    private float r => Input.GetAxis("Mouse ScrollWheel");
    [SerializeField] private float speed = 10.0f;

    void Start()
    {
        tr = transform;
        this.enabled = tr.root.GetComponent<PhotonView>().IsMine;
    }

    // Update is called once per frame
    void Update()
    {
        tr.Rotate(Vector3.right * Time.deltaTime * r * speed);
    }
}
