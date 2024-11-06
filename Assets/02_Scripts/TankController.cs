using UnityEngine;

public class TankController : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float turnSpeed = 100.0f;

    private float v => Input.GetAxis("Vertical");
    private float h => Input.GetAxis("Horizontal");

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
        tr.Rotate(Vector3.up * Time.deltaTime * h * turnSpeed);
    }
}
