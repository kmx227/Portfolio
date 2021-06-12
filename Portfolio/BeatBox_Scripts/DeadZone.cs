using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    Result theResult;
    NoteManager theNote;

    // Start is called before the first frame update

    private void Start()
    {
        theResult = FindObjectOfType<Result>();
        theNote = FindObjectOfType<NoteManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            theResult.ShowResult(); // box가 추락해서 닿으면 점수판 호출
            theNote.RemoveNote();
        }
        if (other.CompareTag("BasicPlate"))
        {
            Debug.Log("타일 추락");     
            other.gameObject.SetActive(false); //타일 추락하여 닿으면 비활성화
        }
    }
}
