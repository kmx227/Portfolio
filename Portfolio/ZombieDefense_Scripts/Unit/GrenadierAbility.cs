using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierAbility : MonoBehaviour, IUnitAbility
{
    [SerializeField] private GameObject _nearestZombie;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _grenade;
    private bool _isAttack;
    private float _count;

    public void Ability(float _damage, float _range)
    {
        _nearestZombie = CheckToZombie(_range);

        if (_nearestZombie != null)
        {
            var _attackDir = _nearestZombie.transform.position - transform.position;
            _anim.SetFloat("MoveX", _attackDir.x);
            _anim.SetFloat("MoveY", _attackDir.y);  // 공격 대상을 바라보도록 애니메이터 블랜드의 파라미터값 조정

            _count += 1f * Time.deltaTime;
            if (_count >= 3f)
            {
                _count = 0f;
            }

            if (Mathf.Approximately(_count, 0f))
            {
                _anim.SetTrigger("Shoot");
                _anim.ResetTrigger("Reload");
                var _bull = Instantiate(_grenade, _nearestZombie.transform.position, Quaternion.identity);
                _bull.GetComponent<Grenade>().Damage = _damage;
                StartCoroutine(AnimationOff());
            }

            if (!_nearestZombie.activeSelf)
            {
                _nearestZombie = null;
            }
        }
    }

    IEnumerator AnimationOff()
    {
        yield return new WaitForSeconds(0.5f);
        _anim.ResetTrigger("Shoot");
        _anim.SetTrigger("Reload");
    }

    private GameObject CheckToZombie(float _range)
    {
        var _colliders = Physics2D.OverlapCircleAll(transform.position, _range);
        foreach (Collider2D _hit in _colliders)
        {
            if (_hit.GetComponent<Zombie>())
            {
                if (_nearestZombie == null)
                {
                    return _hit.gameObject;
                }
                else
                {
                    return _nearestZombie;
                }
            }
            else
            {
                _nearestZombie = null;
            }
        }

        return _nearestZombie;
        // 나를 기점으로 원 모양의 검출기를 만들어 collider2D 오브젝트들을 검출하여 그 중에서 Zombie 컴포넌트를 포함한 좀비를 검출
    }
}
