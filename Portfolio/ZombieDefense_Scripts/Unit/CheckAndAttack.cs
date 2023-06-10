using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAndAttack : MonoBehaviour, IUnitAbility
{
	[SerializeField] private GameObject _nearestZombie;
	private GameObject _currentZombie;
	[SerializeField] private Animator _anim;
	private bool _isAttack;
	private AudioSource _audio;

	public GameObject NearestZombie { get => _nearestZombie; set => _nearestZombie = value; }

    private void Awake()
    {
		_audio = GetComponent<AudioSource>();
    }

    public void Ability(float _damage, float _range)
    {
		_nearestZombie = CheckToZombie(_range);

		if(_nearestZombie != null)
        {
			var _attackDir = _nearestZombie.transform.position - transform.position;
			_anim.SetFloat("MoveX", _attackDir.x);
			_anim.SetFloat("MoveY", _attackDir.z);  // ���� ����� �ٶ󺸵��� �ִϸ����� ������ �Ķ���Ͱ� ����

			_isAttack = true;
			_anim.SetBool("isAttack", _isAttack);
			DamageTo(_nearestZombie, _damage);
            if (!_audio.isPlaying)
            {
				_audio.mute = false;
				_audio.Play();
            }

			if (!_nearestZombie.activeSelf)
			{
				_nearestZombie = null;
			}
		}
        else
        {
			_isAttack = false;
			_anim.SetBool("isAttack", _isAttack);
            if (_audio.isPlaying)
            {
				_audio.mute = true;
				_audio.Stop();
			}
		}
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
		// ���� �������� �� ����� ����⸦ ����� collider2D ������Ʈ���� �����Ͽ� �� �߿��� Zombie ������Ʈ�� ������ ���� ����
	}

	private void DamageTo(GameObject _nearestZombie, float _damage)
	{
		_nearestZombie.GetComponent<Zombie>().Hp -= _damage * Time.deltaTime;
	}
}
