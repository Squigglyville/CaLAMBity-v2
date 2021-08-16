using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{

    public AudioSource playSound;

    void OnTriggerEnter(Collider other)
    {
        playSound.Play();
    }

}
