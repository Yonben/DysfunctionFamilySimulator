using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundExample : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        
    }
    public void playSound()
    {
        audioSource.Play();
        print("sound");
    }
}
