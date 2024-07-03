using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int bombNum;
    [SerializeField] private GameObject target;

    private float Hp;
    private int Exp;
    private Animation anim;
    private Vector2 pos;
    private bool isAttacked = false;

    private void Awake()
    {
        Hp = 1;
        Exp = 10;
        anim = GetComponent<Animation>();
    }

    public void SetPos(Vector2 pos)
    {
        this.pos = pos;
    }

    public void InitBomb(bool attacked)
    {
        anim.Stop();
        var newPos = new Vector2(target.transform.position.x + pos.x, target.transform.position.y + pos.y);
        transform.position = newPos;
        if (attacked) 
        {
            isAttacked = true;
            GameManager.instance.AddExp(Exp);
        }
        else
        {
            if (isAttacked) return;

            if(bombNum == 0) GameManager.instance.GameHPDamaged(false);
        }
    }

    public void PlayAnim()
    {
        isAttacked = false;
        anim.Play();
    }

    public bool FinishedAnim()
    {
        return anim.isPlaying;
    }
}
