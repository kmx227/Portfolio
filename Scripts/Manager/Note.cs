using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400;

    UnityEngine.UI.Image noteImage;

    TimingManager theTimingManager;


    private void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
    }
    //반납 후 다시 가져오기 위해
    void OnEnable() //객체가 호출될때마다 함수 호출
    {
        if (noteImage == null)
        {
            noteImage = GetComponent<UnityEngine.UI.Image>();
        }

        noteImage.enabled = true;
    }

    //노트를 사라지게 만듬
    public void HideNote()
    {
        noteImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public bool GetNoteFlag()
    {
        //노트remove에서 노트를 파괴하지않고 이미지만 비활성화시켰기때문에 miss가 올바르지않은때에 뜨는 문제 발생
        //화면밖으로 나간 노트들에 miss를띄울떄 enable이 true일때만 실행시키면됨
        return noteImage.enabled;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Miss"))
        {

            Debug.Log("!");
            //노트가 삭제될대도 노트리스트에서 제거
           // theTimingManager.boxNoteList.Remove(collision.gameObject);
         
            //ObjectPool.instance.noteQueue.Enqueue(collision.gameObject); //사용 후 반납
           // collision.gameObject.SetActive(false); //비활성화
        }
    }
}
