using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float force = 1500.0f;
    [SerializeField] private GameObject expEffect;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force);
    }

    void OnCollisionEnter()
    {
        var obj = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(obj, 5.0f);

        Destroy(this.gameObject);
    }
}
