using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;

    [SerializeField] Text[] txtCount = null; 
    [SerializeField] Text txtCoin = null;
    [SerializeField] Text txtScore = null;
    [SerializeField] Text txtMaxCombo = null; // 점수표기해 주기 위해 설정

    int currentSong = 0;
    public GameObject btnMenu;
    public GameObject btn;

    public void SetCurrentSong(int p_songNum)
    {
        currentSong = p_songNum;
    }

    ScoreManager theScore; 
    ComboManager theCombo;
    TimingManager theTiming;
    DataBase_Manager theDatabase;
    // Start is called before the first frame update
    void Start()
    {
        theScore = FindObjectOfType<ScoreManager>();
        theCombo = FindObjectOfType<ComboManager>();
        theTiming = FindObjectOfType<TimingManager>();
        theDatabase = FindObjectOfType<DataBase_Manager>();
        Instantiate(btnMenu);
        Instantiate(btn);
    }

    public void ShowResult()
    {
        Invoke("IsCanBtn", 5f);

        FindObjectOfType<CenterFlame>().ResetMusic();
        AudioManager.instance.StopBGM();

        goUI.SetActive(true);

        for(int i =0; i <txtCount.Length; i++)
        { 
            txtCount[i].text = "0"; 
        }
        
        txtCoin.text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        int[] t_judgement = theTiming.GetJudgementRecord();
        int t_currentScore = theScore.GetCurrentScore();
        int t_MaxCombo = theCombo.GetMaxCombo();
        int t_coin = (t_currentScore / 50);

        for(int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]); // 등급별 표현
        }

        txtScore.text = string.Format("{0:#,##0}", t_currentScore); // 점수
        txtMaxCombo.text = string.Format("{0:#,##0}", t_MaxCombo); 
        txtCoin.text = string.Format("{0:#,##0}", t_coin);

        if (t_currentScore > theDatabase.score[currentSong])
        {
            theDatabase.score[currentSong] = t_currentScore;
            theDatabase.SaveScore();
        }

    }

    void IsCanBtn()
    {
        btnMenu.GetComponent<Button>().interactable = true;
        btn.GetComponent<Button>().interactable = true;
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
        theCombo.ResetCombo();
        btnMenu.GetComponent<Button>().interactable = false;
        btn.GetComponent<Button>().interactable = false;
    }

    public void BtnRetry()
    {
        goUI.SetActive(false);
        GameManager.instance.GameStart(currentSong, 130);
        theCombo.ResetCombo();
        btn.GetComponent<Button>().interactable = false;
        btnMenu.GetComponent<Button>().interactable = false;
    }
}
