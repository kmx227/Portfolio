using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    //생성된 노드를 담는 List
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];

    //판정범위의 중심을 보여줌
    [SerializeField] Transform Center = null;

    //다양한 판정범위를 나타냄(Perfect, Cool, Good, Bad)
    [SerializeField] RectTransform[] timingRect = null;

    //TimingBox = 판정 범위의 최소값(x), 최대값(y)
    Vector2[] timingBoxs = null;


    EffectManager theEffect;
    ScoreManager theScoreManager;
    ComboManager theComboManager;
    StageManager theStageManager;
    PlayerController thePlayer; //올바른길로 갔을때만 타일을 생성해야하므로 플레이어의 접근에 대한 참조가 필요
    


    // Start is called before the first frame update
    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();
        //타이밍 박스 설정
        //0번째 TimingBox = Perfect(가장 좁은 판정)
        //3번째 TimingBox = Bad(가장 넓은 판정)

        theScoreManager = FindObjectOfType<ScoreManager>(); 
        
        theComboManager = FindObjectOfType<ComboManager>();
        
        theStageManager = FindObjectOfType<StageManager>();

        thePlayer = FindObjectOfType<PlayerController>();

        //TimingBoxs의 크기 timingRect의 길이만큼
        timingBoxs = new Vector2[timingRect.Length]; 

        //배열이니까 0 ,1 ,2 ,3이겠지 지금 4개니까
        for(int i = 0; i< timingRect.Length; i++)
        {
            //각각의 판정 범위 ->
            //최소값 = 중심 - (이미지의 너비 / 2) 
            //최대값 = 중심 + (이미지의 너비 / 2)

            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2, 
                              Center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }


    //리스트에 있는 노트들을 확인해서 판정 박스에 있는 노트를 찾아야함
    public bool CheckTiming()
    {
        for(int i = 0; i< boxNoteList.Count; i++)
        {

            //각노트의 x값을 따로받아서 이값으로 판정범위 안에 들어왔는지 판단할거임
            //판정범위의 최소값 <= 노트의 x값 <= 판정범위 최대값
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            //판정범위만큼 반복 -> 어느 판정 범위에 있는지 확인
            for(int x = 0; x < timingBoxs.Length; x++)
            {

                //노트의 x값이 판정범위안에 들어와있는지
                if(timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {

                    //노트제거
                    //remove를 안하는 이유는 시작할떄 노트가 음악을 시작시켜주는 범위안에 들어오기전에 판정체크로 사라지면
                    //음악이 재생이 안되기때문에 없에는게 아니라 안보이게 만들어주는거임떄
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    theEffect.NoteHitEffect();
                    boxNoteList.RemoveAt(i);


                    //이펙트연출
                    if (x < timingBoxs.Length - 1)
                        theEffect.NoteHitEffect();

                    

                   


                    if(CheckCanNextPlate())
                    {
                        //점수증가 왜 여기다 넣냐면 똑같은자리 왓다갓다해서 점수 못올리게 해야하니까
                        theScoreManager.IncreaseScore(x); //IncreaseScore함수 호출
                        theStageManager.ShowNextPlate();
                        //theStageManager.FallPlate();

                        //X의값을 파라미터로 넘겨(만약 Bad타입 때 가중치를 넣고 싶지 않으면 이펙트 연출 if함수 안에 넣으면 됨)
                        theEffect.judgementEffect(x); // 판정 연출
                        judgementRecord[x]++; // 판정 기록
                    }
                    else
                    {
                        theEffect.judgementEffect(5);
                    }

                    AudioManager.instance.PlaySFX("Clap");

                    return true;
                }

            }
        }
        //콤보 리셋
        theComboManager.ResetCombo();
        //노트 못찾았으면 miss겠지  4넣음
        theEffect.judgementEffect(timingBoxs.Length);
        MissRecord(); // miss 기록
        return false;
    }   

    bool CheckCanNextPlate()
    {
        //가상의 광선을쏴서 맞은 대상의 정보를 가져옴
        if (Physics.Raycast(thePlayer.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate") || t_hitInfo.transform.CompareTag("Tile"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;
                }
            }
        }
        return false;
    }

    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        judgementRecord[4]++;
    }

    public void Initialized()
    {
        judgementRecord[0] = 0;
        judgementRecord[1] = 0;
        judgementRecord[2] = 0;
        judgementRecord[3] = 0;
        judgementRecord[4] = 0;
    }
}
