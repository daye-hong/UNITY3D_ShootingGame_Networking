using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class fireBullet : MonoBehaviourPunCallbacks
{
    public Transform firepos;
    //public GameObject fire;
    public GameObject bullet;
    AudioSource theAudio;

    void Start()
    {
        theAudio = GameObject.Find("Female(Clone)").GetComponent<Sound>().theAudio;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().Change == 2)
            {
                theAudio.clip = GameObject.Find("Female(Clone)").GetComponent<Sound>().clip[0];
                theAudio.Play();
                Debug.Log("Female fire");
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
