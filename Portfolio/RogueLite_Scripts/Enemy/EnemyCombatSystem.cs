using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatSystem : MonoBehaviour
{
    [SerializeField] private float HP = 30f;
    private Rigidbody _rigid;
    private Material _mat;
 
    public void Damage(float _damage, Vector3 _reactDir)
    {
        HP -= _damage;

        if (HP > 0)
        {
            StartCoroutine(ChangeColor());
        }
        else
        {
            _mat.color = Color.red;
            _reactDir = _reactDir.normalized;
            _reactDir = new Vector3(_reactDir.x, 1f, _reactDir.z);
            _rigid.AddForce(_reactDir * 5f, ForceMode.Impulse);
            StartCoroutine(EnemyDeath());
        }
    }

    IEnumerator ChangeColor()
    {
        _mat.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        _mat.color = Color.white;
    }

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        print("Death");
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _mat = GetComponent<MeshRenderer>().material;
    }

    private void FixedUpdate()
    {
        //_rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            var _reactDir = transform.position - other.transform.position;
            Damage(10f,_reactDir);
        }
        else if (other.gameObject.layer == 12)
        {
            print("Parrying");
        }
    }
}
