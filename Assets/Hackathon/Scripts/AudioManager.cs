using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource myAudio;
    public AudioClip[] audioClipArray;

    // Start is called before the first frame update
    void Start()
    {
        References.audioManager = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
