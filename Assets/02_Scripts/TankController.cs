//#pragma warning disable CS0108

/*

ef3e59d8-a553-405a-811f-dcc571ccc892

*/
using System;
using UnityEngine;
using Photon.Pun;
using Unity.Cinemachine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

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
    [SerializeField] private Image hpBar;

    private TMP_Text nickNameText;

    private int initHp = 100;
    private int currHp = 100;

    private float v => Input.GetAxis("Vertical");
    private float h => Input.GetAxis("Horizontal");
    private bool isFire => Input.GetMouseButtonDown(0);

    private List<Renderer> renderers = new List<Renderer>();

    private bool isDie = false;

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        nickNameText = transform.Find("Canvas/Panel/Text - NickName").GetComponent<TMP_Text>();

        GetComponentsInChildren<Renderer>(renderers);

        rb.isKinematic = !pv.IsMine;

        if (pv.IsMine)
        {
            cinemachineCamera.Target.TrackingTarget = tr;
        }

        // 닉네임 설정
        nickNameText.text = pv.Owner.NickName;
    }

    void Update()
    {
        if (pv.IsMine == false) return;

        if (isDie) return;

        tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
        tr.Rotate(Vector3.up * Time.deltaTime * h * turnSpeed);

        if (isFire)
        {
            //Fire();
            pv.RPC(nameof(Fire), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void Fire()
    {
        Instantiate(cannonPrefab, firePos.position, firePos.rotation);
        audio?.PlayOneShot(fireSfx, 0.8f);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (isDie) return;

        if (coll.collider.CompareTag("CANNON"))
        {
            currHp -= 20;
            hpBar.fillAmount = (float)currHp / (float)initHp;

            if (currHp <= 0)
            {
                TankDestroy();
            }
        }
    }

    private void TankDestroy()
    {
        isDie = true;

        // 탱크 비활성화
        SetVisibleTank(false);

        Invoke(nameof(RespawnTank), 3.0f);
    }

    private void RespawnTank()
    {
        currHp = initHp;
        hpBar.fillAmount = 1.0f;

        SetVisibleTank(true);
        isDie = false;
    }


    private void SetVisibleTank(bool isVisible)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].enabled = isVisible;
        }

        //GetComponent<BoxCollider>().enabled = isVisible;

        tr.GetComponentInChildren<Canvas>().enabled = isVisible;
    }
}