using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class StartScreen : MonoBehaviour
{
    public SpriteRenderer ground;
    public Sprite secondSprite;

    private int state = 0;
    // Update is called once per frame
    void Update()
    {
        if (XCI.GetButtonDown(XboxButton.A))
        {
            if (state == 0)
            {
                ground.sprite = secondSprite;
            }
            else
            {
                SceneManager.LoadScene("level");
            }
            state++; 
        }
    }
}
