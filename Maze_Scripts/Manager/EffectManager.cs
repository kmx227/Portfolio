using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    //박자맞춰서 눌렀을때 나오는 이펙트
    [SerializeField] Animator noteHitAnimator = null;
    string hit = "Hit";

    [SerializeField] Animator judgementAnimator = null;

    //판정에따라 perfect, cool, normal, bad가 떠야댐

    //교체할 이미지 변수
    [SerializeField] UnityEngine.UI.Image judgementImage = null;

    //4개 판정 담을 변수
    [SerializeField] Sprite[] judgementSprite = null;

    public void judgementEffect(int p_num)
    {
        //파라미터 값에 맞는 판정 이미지 스프라이트로 교체
        judgementImage.sprite = judgementSprite[p_num];
        judgementAnimator.SetTrigger(hit);
    }


    public void NoteHitEffect()
    {
        //Trigger조건으로 hit 파마리터를 넘겨줌
        noteHitAnimator.SetTrigger(hit);
    }


}
