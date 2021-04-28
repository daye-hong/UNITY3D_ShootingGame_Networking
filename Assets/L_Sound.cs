using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Sound : MonoBehaviour
{
    public AudioSource theAudio;
    [SerializeField] public AudioClip[] clip;

    void Start()
    {
        theAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
