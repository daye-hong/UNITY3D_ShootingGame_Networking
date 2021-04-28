using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Hammer : MonoBehaviourPunCallbacks
{
    AudioSource theAudio;
    public Transform KnifeAttackPos;
    public GameObject KnifeAttackBox;

    void Start()
    {
        theAudio = GameObject.Find("Female(Clone)").GetComponent<Sound>().theAudio;
    }

    
    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().Change == 1)
            {
                theAudio.clip = GameObject.Find("Female(Clone)").GetComponent<Sound>().clip[2];
                theAudio.Play();
                GetComponent<Animator>().Play("attack");
                GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().AN.SetBool("attack", true);
                photonView.RPC("KnifeAttack", RpcTarget.Others, null);
                KnifeAttack();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().Change == 1)
            {
                GetComponent<Animator>().Play("attack");
                GameObject.Find("Female(Clone)").GetComponent<MoveCtrl>().AN.SetBool("attack", false);
            }
        }
    }
    [PunRPC]
    void KnifeAttack()
    {
        Instantiate(KnifeAttackBox, KnifeAttackPos.position, KnifeAttackPos.rotation);
    }
}
