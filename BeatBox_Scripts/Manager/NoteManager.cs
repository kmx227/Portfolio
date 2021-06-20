using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    [SerializeField] Transform tfNoteAppear = null;

    //TimingManager참조1
    TimingManager theTimingManager;

    //EffectManager참조1
    EffectManager theEffectManager;

    //Combo 참조
    ComboManager theComboManager;

    StageManager theStageManager;

    private void Start()
    {
        //EffectManager참조2
        theEffectManager = FindObjectOfType<EffectManager>();
        //TimingManager참조2
        theTimingManager = GetComponent<TimingManager>();

        //Combo 참조
        theComboManager = FindObjectOfType<ComboManager>();

        theStageManager = FindObjectOfType<StageManager>();
    }


    // Update is called once per frame
    void Update()
    {

        //noteActive는 박스가 최종목적지에 도달하면 false로 바뀜 = 최종목적지에 박스가 도달하면 노트생성 중단 > noteActive 다른방식으로 변경
        if (GameManager.instance.isStartGame)
        {
            //60 /120 = 1beat당 0.5초라는거 , 60s / bpm = 1beat 시간
            currentTime += Time.deltaTime;

            if (currentTime >= 60d / bpm)
            {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue(); //queue에 담긴 객체 정보 빼옴
                t_note.transform.position = tfNoteAppear.position; //위치정보 값 입력
                t_note.SetActive(true); //활성화

                //노트가 생성되는순간 노트리스트에 해당 노트 추가
                theTimingManager.boxNoteList.Add(t_note);

                //theStageManager.DownPlate();

                currentTime -= 60d / bpm;
                //영상 7분부터다시작업
            }
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            //노트가 화면밖으로 나가면 4를넘겨서 miss를 띄움
            //if (collision.GetComponent<Note>().GetNoteFlag())
            //{
                //theTimingManager.MissRecord();
                //theEffectManager.judgementEffect(4);
                //theComboManager.ResetCombo();
            //}

            //노트가 삭제될대도 노트리스트에서 제거
            theTimingManager.boxNoteList.Remove(collision.gameObject);

            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject); //사용 후 반납
            collision.gameObject.SetActive(false); //비활성화
        }

        if (collision.CompareTag("Miss"))
        {

            Debug.Log("!");
            //노트가 삭제될대도 노트리스트에서 제거
            // theTimingManager.boxNoteList.Remove(collision.gameObject);

            //ObjectPool.instance.noteQueue.Enqueue(collision.gameObject); //사용 후 반납
            // collision.gameObject.SetActive(false); //비활성화
        }

    }

    //박스가 최종 목적지에 도착했을때 노트를 없엠
    public void RemoveNote()
    {

        GameManager.instance.isStartGame = false;

        for(int i =0; i< theTimingManager.boxNoteList.Count; i++)
        {
            theTimingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(theTimingManager.boxNoteList[i]);
        }

        theTimingManager.boxNoteList.Clear();
    }
}
