using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class M_fireBullet : MonoBehaviourPunCallbacks
{
    public Transform firepos;
    //public GameObject fire;
    public GameObject bullet;
    AudioSource theAudio;

    void Start()
    {
        theAudio = GameObject.Find("Male(Clone)").GetComponent<M_Sound>().theAudio;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().Change % 2 != 0)
            {
                theAudio.clip = GameObject.Find("Male(Clone)").GetComponent<M_Sound>().clip[0];
                theAudio.Play();
                GetComponent<Animator>().Play("fire");
                photonView.RPC("Fire", RpcTarget.Others, null);
                Fire();
            }
        }
    }

    [PunRPC]
    void Fire()
    {
        Instantiate(bullet, firepos.position, firepos.rotation);
    }

}
