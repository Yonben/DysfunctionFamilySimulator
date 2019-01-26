using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    public AudioSource audioSourceStart;
    public AudioSource audioSourceStartPanic;
    public AudioSource audioSourcePanic;
    private bool notPanicked = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSourceStart.Play();
        print("2nd music");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playerAliveCount > 3)
        {
            if (!audioSourceStart.isPlaying)
            {
                audioSourceStart.Play();
            }
        }
        else
        {
            if (notPanicked)
            {
                audioSourceStart.Stop();
                audioSourceStartPanic.Play();
                notPanicked = false;
            }
            if (!audioSourceStartPanic.isPlaying && !audioSourcePanic.isPlaying)
            {
                audioSourcePanic.Play();
            }
        }
        


    }
}
