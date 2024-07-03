using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSecond : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private Animation alertAnim;
    [SerializeField] private Animation handAnim;

    IEnumerator ResetCharacterAnim()
    {
        yield return new WaitForSeconds(1.5f);
        character.transform.position.Set(0f, 0.5f, 0f);
        GameManager.instance.CheckParamToNextOfPhase();
    }

    public void PhaseStart(int phase)
    {
        if(phase != 1)  return;

        alertAnim.Play();
    }

    public void PlayHandAnim()
    {
        handAnim.Play();

        StartCoroutine(ResetCharacterAnim());
    }
}
