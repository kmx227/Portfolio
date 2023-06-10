using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Collider2D[] _colliders;
    private float _damage;

    public float Damage { get => _damage; set => _damage = value; }


    // Start is called before the first frame update
    void Start()
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

        StartCoroutine(AttackGrenade());
    }

    IEnumerator AttackGrenade()
    {
        foreach(Collider2D _collider in _colliders)
        {
            if (_collider.GetComponent<Zombie>())
            {
                _collider.GetComponent<Zombie>().Hp -= _damage;
            }
        }

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
