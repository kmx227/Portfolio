using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PhaseThird : MonoBehaviour
{
    [SerializeField] Bomb[] bombs;
    [SerializeField] GameObject target;
    [SerializeField] private Vector2[] pos;
    [SerializeField] private AnimationClip[] clips;
    [SerializeField] private Animation alertAnim;

    private Animation anim;
    private bool isStartPhase = false;
    private bool onBomb = false;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<bombs.Length; i++)
        {
            bombs[i].SetPos(pos[i]);
        }
    }

    // bomb 애니메이션이 종료 되면 초기화
    IEnumerator ResetCharacterAnim()
    {
        yield return new WaitUntil(() => (bombs[0].FinishedAnim() == false && bombs[1].FinishedAnim() == false && bombs[2].FinishedAnim() == false));
        for(int i = 0; i < bombs.Length; i++)
        {
            bombs[i].InitBomb(false);
        }
        GameManager.instance.CheckParamToNextOfPhase();
    }

    public void StartPhase(int phase)
    {
        if (phase != 3) return;
        alertAnim.Play();
    }

    public void PlayBombAnim()
    {
        for (int i = 0; i < bombs.Length; i++)
        {
            bombs[i].PlayAnim();
        }

        StartCoroutine(ResetCharacterAnim());
    }

    public void Bomb()
    {
        onBomb = true;
    }
}
