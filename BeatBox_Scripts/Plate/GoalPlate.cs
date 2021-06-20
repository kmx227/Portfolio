using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource theAudio;
    NoteManager theNote;

    Result theResult;
    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
        theNote = FindObjectOfType<NoteManager>();
        theResult = FindObjectOfType<Result>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //최종목적지에 도착하면 음악 실행
        if (other.CompareTag("Player"))
        {
            theAudio.Play();
            PlayerController.s_canPresskey = false;
            theNote.RemoveNote();
            theResult.ShowResult();
        }
    }
}
