using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float force = 1500.0f;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force);
    }
}
