using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{

    bool musicStart = false;
    public string bgmName = "";

    public void ResetMusic()
    {
        musicStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //음악은 첫 퀄라이더 충돌때만 실행해야하므로 boolean변수를 하나 만든거임
        if (!musicStart)
        {

        
           if (collision.CompareTag("Note"))
           {
                AudioManager.instance.PlayBGM(bgmName);
                musicStart = true;
           }
        }
    }

}
