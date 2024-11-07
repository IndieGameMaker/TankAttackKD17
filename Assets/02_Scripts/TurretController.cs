using Photon.Pun;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private Transform tr;
    [SerializeField] private float turnSpeed = 50.0f;

    void Start()
    {
        tr = GetComponent<Transform>();

        this.enabled = tr.root.GetComponent<PhotonView>().IsMine;
    }

    void Update()
    {
        // 메인카메라에서 Ray 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << 8))
        {
            // 터렛 기준으로 hit 월드좌표 -> 로컬좌표로 변환
            Vector3 pos = tr.InverseTransformPoint(hit.point);
            // Atan2 두 좌표간의 각도를 계산 Atan(pos.x/pos.z)
            float angle = Mathf.Atan2(pos.x, pos.z) * Mathf.Rad2Deg;

            // 터렛 회전
            tr.Rotate(Vector3.up * angle * Time.deltaTime * turnSpeed);
        }
    }
}
