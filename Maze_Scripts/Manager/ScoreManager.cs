using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text txtScore = null; //text변수

    [SerializeField] int increaseScore = 10; //맞출 때마다 점수 증가 default변수
    int currentScore = 0; //현재 점수를 담을 변수

    [SerializeField] float[] weight = null; //등급에 따라 다른 가중치 배열
    [SerializeField] int comboBonusScore = 10;

    //Animator 변수 설정
    Animator myAnim;
    string animScoreUp = "ScoreUp";

    //콤보 manager참조
    ComboManager theCombo;

    // Start is called before the first frame update
    void Start()
    {
        theCombo = FindObjectOfType<ComboManager>();
        myAnim = GetComponent<Animator>();
        currentScore = 0;
        txtScore.text = "0"; //시작과 동시에 초기화
    }

    public void Initailized()
    {
        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        theCombo.IncreaseCombo(); // 콤보 증가

        //콤보 보너스 계산
        int t_currentCombo = theCombo.GetCurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;


        int t_increaseScore = increaseScore + t_bonusComboScore; //임시 변수에 증가 된 점수 입력
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]); // 판정에 따른 가중치 부여

        currentScore += t_increaseScore; //부여된 점수를 현재 점수에 더함
        txtScore.text = string.Format("{0:#,##0}", currentScore);
        //현재 점수를 text에 특정 포맷으로 띄움

        myAnim.SetTrigger(animScoreUp); //애니메이션 실행
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    private int GetCurrentCombo()
    {
        throw new NotImplementedException();
    }
}
