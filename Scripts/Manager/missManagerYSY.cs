using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missManagerYSY : MonoBehaviour
{
    //TimingManager참조1
    TimingManager theTimingManager;

    //EffectManager참조1
    EffectManager theEffectManager;

    //Combo 참조
    ComboManager theComboManager;

    //[설명]
    // 노트의 퀄라이더와 missrect의 퀄라이더가 충돌할경우 
    // 콤보의 점수를 초기화
    // miss이펙트를 연출
    // 또한 게임오브젝트를 비활성화시키면 퀄라이더가 작동하지않으므로 투명도를 0으로 설정하여 안보이는것처럼 만듦

    // Start is called before the first frame update
    void Start()
    {
        //EffectManager참조2
        theEffectManager = FindObjectOfType<EffectManager>();
        //TimingManager참조2
        theTimingManager = GetComponent<TimingManager>();
        //Combo 참조
        theComboManager = FindObjectOfType<ComboManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            //노트가 화면밖으로 나가면 4를넘겨서 miss를 띄움  
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                //theTimingManager.MissRecord(); 어차피 화면밖으로나가면찍어줄테니까 여기서안해도됨, miss회수를 보여주는건 아예 없에버리는게 좋을듯
                theEffectManager.judgementEffect(4);
                theComboManager.ResetCombo();
            }
        }
    }
}
