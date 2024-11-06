//#pragma warning disable CS0108

using System;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private new AudioSource audio;

    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float turnSpeed = 100.0f;
    [SerializeField] private GameObject cannonPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioClip fireSfx;

    private float v => Input.GetAxis("Vertical");
    private float h => Input.GetAxis("Horizontal");
    private bool isFire => Input.GetMouseButtonDown(0);

    /*  goes to
    (파라메터) => 문장;
    */

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

        // Func<int, int, int> add = (a, b) => a + b;
        // int sum = add(2, 3);
    }

    void Update()
    {
        tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
        tr.Rotate(Vector3.up * Time.deltaTime * h * turnSpeed);

        if (isFire)
        {
            Fire();
        }
    }

    private void Fire()
    {
        Instantiate(cannonPrefab, firePos.position, firePos.rotation);
        audio.PlayOneShot(fireSfx, 0.8f);
    }
}
