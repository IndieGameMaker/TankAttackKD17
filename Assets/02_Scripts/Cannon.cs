using System;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private CannonDataSO cannonDataSO;

    // Shooter ID
    [NonSerialized]
    [HideInInspector]
    public int shooterId;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * cannonDataSO.force);
        Destroy(this.gameObject, 10.0f);
    }

    void OnCollisionEnter()
    {
        var obj = Instantiate(cannonDataSO.expEffect, transform.position, Quaternion.identity);
        Destroy(obj, 5.0f);

        Destroy(this.gameObject);
    }
}
