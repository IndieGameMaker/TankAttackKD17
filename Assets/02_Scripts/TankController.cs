//#pragma warning disable CS0108

/*

ef3e59d8-a553-405a-811f-dcc571ccc892

*/
using System;
using UnityEngine;
using Photon.Pun;
using Unity.Cinemachine;

public class TankController : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private PhotonView pv;
    private new AudioSource audio;
    private CinemachineCamera cinemachineCamera;

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
        pv = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

        rb.isKinematic = !pv.IsMine;

        if (pv.IsMine)
        {
            cinemachineCamera.Target.TrackingTarget = tr;
        }

        // Func<int, int, int> add = (a, b) => a + b;
        // int sum = add(2, 3);
    }

    void Update()
    {
        if (pv.IsMine == false) return;

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