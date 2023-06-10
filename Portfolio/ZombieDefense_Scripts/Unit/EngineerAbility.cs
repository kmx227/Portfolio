using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerAbility : MonoBehaviour, IUnitAbility
{
	[SerializeField] private Animator _anim;
	[SerializeField] private UnitController _unit;
	[SerializeField] private LayerMask layerFence;
	private GameManager _gm;
	private AudioSource _audio;
	private bool _isAttack;

    private void Awake()
    {
		_gm = FindObjectOfType<GameManager>();
		_audio = GetComponent<AudioSource>();
    }

    public void Ability(float _damage, float _range)
    {
		RaycastHit2D _hit_Fence = Physics2D.Raycast(transform.position, _unit.TargetDir, 0.8f, layerFence);
		if (_hit_Fence)
		{
			_isAttack = true;
			_anim.SetBool("isAttack", _isAttack);
            if (!_audio.isPlaying)
            {
				_audio.Play();
			}

			var _attackDir = _unit.TargetDir - (Vector2)transform.position;
			_anim.SetFloat("MoveX", -_attackDir.x);
			_anim.SetFloat("MoveY", -_attackDir.y);

			if (_gm.FenceHp < 1000)
			{
				_gm.FenceHp += _damage * Time.fixedDeltaTime;
				_gm.FenceHpSlider.value += _damage * Time.fixedDeltaTime;
			}
        }
        else
        {
			_isAttack = false;
			_anim.SetBool("isAttack", _isAttack);
		}
	}
}
