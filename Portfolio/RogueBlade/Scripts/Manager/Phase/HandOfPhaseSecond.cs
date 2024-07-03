using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOfPhaseSecond : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)) // 8-> 점프 중인 캐릭터 / 9-> 점프 하지 않은 캐릭터(바닥에 있는)
        {
            collision.GetComponent<Character>().onCollisionByPhaseSecond(animator);
        }
    }
}
