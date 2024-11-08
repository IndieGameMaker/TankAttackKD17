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
using UnityEngine.EventSystems;
using Photon.Realtime;

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

        if (isFire && !EventSystem.current.IsPointerOverGameObject())
        {
            //Fire();
            pv.RPC(nameof(Fire), RpcTarget.AllViaServer, pv.Owner.ActorNumber);
        }
    }

    [PunRPC]
    private void Fire(int shooterId)
    {
        var _cannon = Instantiate(cannonPrefab, firePos.position, firePos.rotation);
        _cannon.GetComponent<Cannon>().shooterId = shooterId;

        audio?.PlayOneShot(fireSfx, 0.8f);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (isDie) return;

        if (coll.collider.CompareTag("CANNON"))
        {
            // ActorNumber -> NickName
            int actorNumber = coll.gameObject.GetComponent<Cannon>().shooterId;
            Player shooter = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);

            currHp -= 20;
            hpBar.fillAmount = (float)currHp / (float)initHp;

            if (currHp <= 0)
            {
                if (pv.IsMine)
                {
                    string msg = $"<color=#00ff00>[{pv.Owner.NickName}]님은 "
                    + $"<color=#00ff00>[{shooter.NickName}]에게 피격당했습니다.";

                    GameManager.Instance.SendMessageByRPC(msg);
                }

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