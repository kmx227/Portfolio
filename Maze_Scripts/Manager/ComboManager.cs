using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    //이미지 변수와 콤보 text 선언
    [SerializeField] GameObject  goComboImage = null;
    [SerializeField] UnityEngine.UI.Text txtCombo = null;

    int currentCombo = 0; //현재의 콤보 변수
    int maxCombo = 0;

    Animator myAnim;
    string animComboUp = "ComboUp";

    //첫 화면에서는 콤보가 나오지 않음
    void Start()
    {
        myAnim = GetComponent<Animator>();
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    //콤보 증가 함수
    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        txtCombo.text = string.Format("{0:#,##0}", currentCombo);

        if(maxCombo < currentCombo)
        {
            maxCombo = currentCombo;
        }

        //콤보와 이미지는 3콤보 이상부터
        if(currentCombo > 2)
        {
            txtCombo.gameObject.SetActive(true);
            goComboImage.SetActive(true);

            myAnim.SetTrigger(animComboUp);
        }

    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txtCombo.text = "0";
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    public int GetMaxCombo()
    {
        return maxCombo;
    }
}
