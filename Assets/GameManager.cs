using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Transform[] Points = GameObject.Find("Player_Point").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, Points.Length);

        if (DataManager.instance.currentCharacter == Character.Male)
        {
            PhotonNetwork.Instantiate("Male", Points[idx].position , Quaternion.Euler(0,180,0));
        }

        if (DataManager.instance.currentCharacter == Character.Female)
        {
            PhotonNetwork.Instantiate("Female", Points[idx].position, Quaternion.Euler(0, 180, 0));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
