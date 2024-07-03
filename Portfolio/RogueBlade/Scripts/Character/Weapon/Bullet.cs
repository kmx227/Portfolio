using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    private GameObject target;
    private Rigidbody2D rigid;
    private Vector3 dir;
    private Quaternion rotTarget;
    private float damage = 0;
    private readonly float speed = 10f;
    private readonly float rotSpeed = 10f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update

    private void FixedUpdate()
    {
        if (target.gameObject.activeSelf == false)
        {
            ObjectPool.ReturnObject(gameObject, 3);
        }

        dir = (target.transform.position - transform.position).normalized;
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        rotTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, rotSpeed * Time.deltaTime);    
        rigid.velocity = new Vector2(dir.x * speed, dir.y * speed);
    }

    public void Init(GameObject target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == 7) // ºí·°
            {

                collision.GetComponent<Block>().OnDamaged(damage);
                ObjectPool.ReturnObject(gameObject, 3);
            }
            else if(collision.gameObject.layer == 10) // Bomb
            {
                collision.GetComponent<Bomb>().InitBomb(true);
            }
        }
    }
}
