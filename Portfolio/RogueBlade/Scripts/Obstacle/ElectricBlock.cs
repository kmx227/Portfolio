using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ElectricBlock : Block
{
    private bool isBlocked = false;
    
    // 쉴드로 한 번 밀어내야 깰 수 있음
    public override void Lift(float power)
    {
        base.Lift(power);
        if (isBlocked == false)
        {
            isBlocked = true;
        }
    }

    public override void OnDamaged(float damage)
    {
        if (isBlocked == false) return;

        base.OnDamaged(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GameManager.instance.GameHPDamaged(false);
        }
        else if (collision.gameObject.layer == 6)
        {
            GameManager.instance.GameHPDamaged(true);
        }
    }
}
