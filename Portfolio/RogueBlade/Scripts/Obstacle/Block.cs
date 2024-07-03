using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Block : MonoBehaviour
{
    Rigidbody2D rid;
    private int blockType;
    private float Hp;
    [SerializeField] private int hasExp;

    private float[] HpOfLevel = { 1f, 2f, 5f, 10f, 15f };
    private int[] ExpOfLevel = { 10, 12, 15, 19, 23 };

    // Start is called before the first frame update
    void OnEnable()
    {
        rid = GetComponent<Rigidbody2D>();
        if (GameManager.instance)
        {
            Hp = HpOfLevel[GameManager.instance.GetCurrentPhase()];
            hasExp = ExpOfLevel[GameManager.instance.GetCurrentPhase()];
        }
    }

    public void InitType(int num)
    {
        blockType = num;
    }

    /// <summary>
    /// 쉴드 스킬로 인해 블록이 위로 띄어짐
    /// </summary>
    /// <param name="power">띄어지는 파워</param>
    public virtual void Lift(float power)
    {
        rid.velocity = Vector2.zero;
        rid.AddForce(Vector2.up * (power * BlockManager.CurrentBlockCount()), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) //캐릭터
        {
            rid.velocity = Vector2.zero;
        }
        else if (collision.gameObject.layer == 6) // 바닥
        {
            GameManager.instance.GameHPDamaged(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9) // 8-> 점프 중인 캐릭터 / 9-> 점프 하지않은 캐릭터
        {
            GameManager.instance.GameHPDamaged(false);
            rid.velocity = Vector2.zero;
            DestroyBlock(false);
        }
    }

    /// <summary>
    /// 블록을 다시 풀에 돌려줌
    /// </summary>
    /// <param name="attacked">공격 받았을 시 경험치 증가</param>
    public void DestroyBlock(bool attacked)
    {
        if (attacked) 
        {
            GameManager.instance.AddExp(hasExp);
        }

        BlockManager.RemoveBlockInCurrentGroup(this);
        ObjectPool.ReturnObject(gameObject, blockType);
    }
    
    public virtual void OnDamaged(float damage)
    {
        Hp -= damage;

        if(Hp <= 0)
        {
            DestroyBlock(true);
        }
    }

    /// <summary>
    /// 메가 점프로 인한 데미지 계산, 현재 currentBlockGroup내의 블록들의 데미지 계산을 한번에 하기 위함
    /// </summary>
    /// <param name="attackDamage">총합 데미지</param>
    /// <param name="damage">첫 번째 블록에 데미지를 준 후 남은 데미지</param>
    /// <returns></returns>
    public bool Onekilled(float attackDamage, out float damage)
    {
        if(attackDamage >= Hp){
            damage = attackDamage - Hp;
            Lift(0);
            OnDamaged(Hp);
            return true;
        }
        else
        {
            damage = Hp - attackDamage;
            Lift(0);
            OnDamaged(damage);
            return false;
        }
    }
}
