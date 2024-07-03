using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private Rigidbody2D rigid;
    private BoxCollider2D boxCol;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isAttackable = true;
    private float megaJumpPower = 20f;
    private int jumpCount = 0;
    private bool canMegaJump = false;

    [SerializeField] private Shield shield;
    [SerializeField] private SpriteRenderer jumpEffect;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Sprite[] hasWeaponSprites;
    [SerializeField] private Text jumpText;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.DrawRay(transform.position, Vector2.up * 2.5f, new Color(1, 0, 0));

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Attack();
        }
        else if(Input.GetKeyDown(KeyCode.Keypad7)) MegaJump();*/

        if(CanJump())
        {
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }*/

            gameObject.layer = 9;
        }
        else
        {
            gameObject.layer = 8;
        }

        if (canMegaJump) jumpText.text = "Mega Jmup";
        else jumpText.text = "Jmup";
    }

    public void Attack()
    {
        if(isAttackable == false) { return; }
        animator.enabled = true;
        animator.Rebind();
        animator.SetTrigger("OnAttack");
        animator.SetInteger("currentWeaponType", weapon.GetWeaponType());

        isAttackable = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, 2.5f, LayerMask.GetMask("Block","Bomb"));

        if (hits.Length > 0)
        {
            if(hits[0].collider.gameObject.layer == 10) // Bomb
            {
                var circles = Physics2D.OverlapCircleAll(transform.position, 2f, LayerMask.GetMask("Bomb"));
                for(int i=circles.Length-1; i>=0; i--)
                {
                    circles[i].GetComponent<Bomb>().InitBomb(true);
                }

                StartCoroutine(TimerForAttack());
                return;
            }

            var array = new GameObject[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                array[i] = hits[i].collider.gameObject;
            }
            weapon.OnAttack(array);
        }

        StartCoroutine(TimerForAttack());
    }

    // 공격 쿨타임
    IEnumerator TimerForAttack()
    {
        yield return new WaitForSeconds(0.2f);
        isAttackable = true;
    }

    // 점프 가능 체크
    public bool CanJump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.3f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
         
            return true;
        }
        else { return false; }
    }

    public void Jump()
    {
        if (CanJump())
        {
            if (canMegaJump)
            {
                MegaJump();
            }
            else
            {
                StartCoroutine(OnJumpEffect());
                rigid.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
                jumpCount++;
                if (jumpCount > 10)
                {
                    jumpCount = 0;
                    canMegaJump = true;
                }
            }
        }
    }

    private void MegaJump()
    {
        var blocks = BlockManager.GetCurrentBlockList();
        var blockQueue = new Queue<Block>();
        foreach (var block in blocks)
        {
            blockQueue.Enqueue(block);
        }

        // 총합 데미지를 가지고 currentBlockGroup내의 블럭들을 순차적으로 데미지 계산
        while(megaJumpPower > 0)
        {
            print(blockQueue.Count);
            if (blockQueue.Count <= 0) break;

            var block = blockQueue.Dequeue();
            var power = 0f;

            if (block.Onekilled(megaJumpPower, out power))
            {
                megaJumpPower = power;
            }
            else
            {
                break;
            }
        }

        StartCoroutine(OnJumpEffect());
        rigid.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
        if(canMegaJump) canMegaJump = false;
    }

    IEnumerator OnJumpEffect()
    {
        jumpEffect.enabled = true;
        yield return new WaitForSeconds(1);
        jumpEffect.enabled = false;
    }

    public void ChangeWeapon(int count)
    {
        animator.enabled = false;
        spriteRenderer.sprite = hasWeaponSprites[count];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            rigid.velocity = Vector2.zero;
        }
    }

    //------------------------------------ 2번째 페이즈를 위한 애니메이션 처리
    public void onCollisionByPhaseSecond(Animator animator)
    {
        if (shield.OnShield() == true)
        {
            GameManager.instance.AddExp(30);
            return;
        }
        spriteRenderer.enabled = false;
        weapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        animator.SetTrigger("PhaseTrigger");
        GameManager.instance.GameHPDamaged(false);

        StartCoroutine(ReVisiableCharacter());
    }

    IEnumerator ReVisiableCharacter()
    {
        yield return new WaitForSeconds(1);
        spriteRenderer.enabled = true;
        weapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
