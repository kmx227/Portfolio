using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private Vector3 _targetPos;
    [SerializeField]
    private LayerMask layerFence;

    [SerializeField] private float _speed; // **
    private Vector3 _targetDir;
    private bool _isZombieMove = true;

    [SerializeField] private float _hp = 100; // ** <- 직렬화 해제 예정
    [SerializeField] private float _zombieDamage = 1f; // ** <- 직렬화 해제 예정
    private float _totalHp = 100;

    private Animator _animator;
    [SerializeField] private Image _hPImage;

    private GameManager _gm;
    private EnemySystem _enemySystem;

    public float Hp { get => _hp; set => _hp = value; }

    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _enemySystem = FindObjectOfType<EnemySystem>();
        _targetDir = _targetPos - transform.position;
        _animator = GetComponent<Animator>();
        _totalHp = _hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isZombieMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetDir, Time.fixedDeltaTime * _speed);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            RaycastHit2D _hit = Physics2D.Raycast(transform.position, _targetDir, 0.5f, layerFence);

            if (_hit)
            {
                // 공격 애니메이션
                _isZombieMove = false;
            }

            _animator.SetFloat("MoveX", _targetDir.x);
            _animator.SetFloat("MoveY", _targetDir.y);
            _animator.SetBool("isMove", _isZombieMove);

            if(Vector3.Distance(transform.position, _targetPos) <= 0.1f){
                _isZombieMove = false;
            }
        }
        else
        {
            if(_gm.FenceHp > 0)
            {
                _gm.FenceHp -= _zombieDamage * Time.deltaTime;
                _gm.FenceHpSlider.value -= _zombieDamage * Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetDir, Time.fixedDeltaTime * _speed);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

                _animator.SetFloat("MoveX", _targetDir.x);
                _animator.SetFloat("MoveY", _targetDir.y);
                _animator.SetBool("isMove", _isZombieMove);
            }
        }

        if(_hp <= 0)
        {
            //Destroy(gameObject);
            
            _gm.Coin += 1;
            _enemySystem.ZombeList.Remove(this);
            _enemySystem.ReturnObject(this);
        }
        else
        {
            _hPImage.fillAmount = _hp / _totalHp;
        }
    }
}
