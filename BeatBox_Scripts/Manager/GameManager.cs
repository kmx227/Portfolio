using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] goGameUI = null;
    [SerializeField] GameObject goTitleUI = null;

    public static GameManager instance;

    public bool isStartGame = false;

    ComboManager theCombo;
    ScoreManager theScore;
    TimingManager thetiming;
    PlayerController thePlayer;
    StageManager theStage;
    NoteManager theNode;
    Result theResult;
    [SerializeField] CenterFlame theMusic = null; //비활성화된 객체는 findobject로 찾을 수 없음
        // Start is called before the first frame update
    void Start()
    {
        instance = this;
        theNode = FindObjectOfType<NoteManager>();
        theStage = FindObjectOfType<StageManager>();
        theCombo = FindObjectOfType<ComboManager>();
        theScore = FindObjectOfType<ScoreManager>();
        thetiming = FindObjectOfType<TimingManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theResult = FindObjectOfType<Result>();
    }

    public void GameStart(int p_songNum, int p_bpm)
    {
        for(int i = 0; i < goGameUI.Length; i++)
        {
            goGameUI[i].SetActive(true);
        }
        theMusic.bgmName = "BGM" + p_songNum;
        theNode.bpm = p_bpm;
        theStage.RemoveStage();
        theStage.SettingStage(p_songNum);
        theCombo.ResetCombo();
        theScore.Initailized();
        thetiming.Initialized();
        thePlayer.Initiallized();
        theResult.SetCurrentSong(p_songNum);

        AudioManager.instance.StopBGM();

        isStartGame = true;
    }


    public void MainMenu()
    {
        for (int i = 0; i < goGameUI.Length; i++)
        {
            goGameUI[i].SetActive(false);
        }

        goTitleUI.SetActive(true);
    }
}
