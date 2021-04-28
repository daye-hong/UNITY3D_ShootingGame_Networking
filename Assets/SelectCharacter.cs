using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SelectCharacter : MonoBehaviourPunCallbacks
{
    public Character character;
    public SelectCharacter[] chars;
    AudioSource theAudio;

    void Start()
    {
        if (DataManager.instance.currentCharacter == character) OnDeSelect();
        else OnDeSelect();
        theAudio = GameObject.Find("Sound").GetComponent<L_Sound>().theAudio;
    }

    private void OnMouseUpAsButton()
    {
        DataManager.instance.currentCharacter = character;
        OnSelect();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] != this) chars[i].OnDeSelect();
        }
    }

    void OnDeSelect()
    {
        transform.localScale = new Vector3(2.5f, 2.5f, 0.2f);
    }
    void OnSelect()
    {
        transform.localScale = new Vector3(3, 3, 0.2f);
        theAudio.clip = GameObject.Find("Sound").GetComponent<L_Sound>().clip[0];
        theAudio.Play();
    }

}
