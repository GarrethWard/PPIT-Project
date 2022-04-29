using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioClip pickupSound, deadSound, spawnPU;

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayPickUpSound()
    {
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
    }
    public void PlayDeadSound()
    {
        AudioSource.PlayClipAtPoint(deadSound, transform.position);
    }

    public void PlaySpawnPickUpSound()
    {
        AudioSource.PlayClipAtPoint(spawnPU, transform.position);
    }
}
