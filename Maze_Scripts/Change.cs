using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class Change : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject goStageUI = null;
    public GameObject Monster;

    // Update is called once per frame
    public string m_Timer = @"00:00:00.000";
    private bool m_IsPlaying;
    public float m_TotalSeconds = 60; // 카운트 다운 전체 초(5분 X 60초), 인스펙트 창에서 수정해야 함. 
    public Text m_Text;
    public bool onText;

    FadeIn Fade;
    private void Start()
    {
        m_Timer = CountdownTimer(false); // Text에 초기값을 넣어 주기 위해
        Fade = FindObjectOfType<FadeIn>();
    }

    private void Update()
    {
        m_IsPlaying = true;
        if (m_IsPlaying)
        {
            m_Timer = CountdownTimer();
        }

        // m_TotalSeconds이 줄어들때, 완전히 0에 맞출수 없기 때문에  
        if(m_TotalSeconds == 1)
        {
            Fade.Fade();
        }
        else if (m_TotalSeconds <= 0)
        {
            SetZero();
            changeMap();
        }

        if (onText)
        {
            m_Text.text = m_Timer;
            goStageUI.GetComponent<Change>().onText = true;
        }
    }

    private string CountdownTimer(bool IsUpdate = true)
    {
        if (IsUpdate)
            m_TotalSeconds -= Time.deltaTime;

        TimeSpan timespan = TimeSpan.FromSeconds(m_TotalSeconds);
        string timer = string.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);

        return timer;
    }

    private void SetZero()
    {
        m_Timer = @"00:00:00.000";
        m_TotalSeconds = 0;
        m_IsPlaying = false;
    }


    void changeMap()
    {
        goStageUI.SetActive(true);
        this.gameObject.SetActive(false);
        m_TotalSeconds = 60;

        Monster.SetActive(true);
    }
}
