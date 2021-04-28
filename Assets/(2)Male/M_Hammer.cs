
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class M_Hammer : MonoBehaviourPunCallbacks
{
    AudioSource theAudio;
    public Transform HammerAttackPos;
    public GameObject HammerAttackBox;

    void Start()
    {
        theAudio = GameObject.Find("Male(Clone)").GetComponent<M_Sound>().theAudio;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().Change % 2 == 0)
            {
                theAudio.clip = GameObject.Find("Male(Clone)").GetComponent<M_Sound>().clip[2];
                theAudio.Play();
                GetComponent<Animator>().Play("attack");
                GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().AN.SetBool("attack", true);
                photonView.RPC("HammerAttack", RpcTarget.Others, null);
                HammerAttack();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().Change % 2 == 0)
            {
                GetComponent<Animator>().Play("attack");
                GameObject.Find("Male(Clone)").GetComponent<M_MoveCtrl>().AN.SetBool("attack", false);
            }
        }
    }
    [PunRPC]
    void HammerAttack()
    {
        Instantiate(HammerAttackBox, HammerAttackPos.position, HammerAttackPos.rotation);
    }

}
