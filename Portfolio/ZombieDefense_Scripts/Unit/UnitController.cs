using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum UnitType
{
	Rifle,
	Sniper,
	Chemical,
	Engineer
}

public class UnitController : MonoBehaviour
{
	//------------------------- 레이어 목록 ---------------------------------------------------------------------
	[SerializeField]
	private GameObject unitMarker;
	[SerializeField]
	private LayerMask layerUnit;
	[SerializeField]
	private LayerMask layerFence;
	[SerializeField]
	private LayerMask layerZombie;
	[SerializeField]
	private UnitType _type;
	[SerializeField]
	private GameObject _grenade;

	//------------------------- 움직임 --------------------------------------------------------------------------
	private SpriteRenderer _sprite;
	private Vector3 _nextMovePosition;
	private Vector3 _targetPos;
	private Vector2 _targetDir;
	private Animator _animator;
	Collider2D[] _colliders;
	[SerializeField] private bool _isMoved = false; // **
	[SerializeField] private float _speed; // **

	//------------------------- 공격 ------------------------------------------------------------------------------
	[SerializeField] private int _unitGrade; // **
	[SerializeField] private bool _isAttack = false; // **
	[SerializeField] private float _attackRange = 3f; // **
	[SerializeField] private GameObject _nearestZomibe;  // **
	[SerializeField] private float _attackDamage = 10; // **
	[SerializeField] private float _count = 0f; // ** <- 직렬화 해제 예정
												// public List<GameObject> _nearZombieListForChemical = new List<GameObject>();

	//------------------------- 유닛 정보 ----------------------------------------------------------------------------
	[SerializeField] private string _unitName;

	public UnitData unitData;
	public Unit _unitType;
	private GameManager _gm;
	private AudioSource _audio;

	public int UnitGrade { get => _unitGrade; set => _unitGrade = value; }
	public UnitType Type { get => _type; }
	public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public Vector2 TargetDir { get => _targetDir; set => _targetDir = value; }
    public string UnitName { get => _unitName; set => _unitName = value; }

    public void Awake()
    {
        _animator = GetComponent<Animator>();
		_colliders = Physics2D.OverlapCircleAll(transform.position, _attackRange);
		_sprite = GetComponent<SpriteRenderer>();
		_gm = FindObjectOfType<GameManager>();
		_audio = GetComponent<AudioSource>();
	}
    private void Start()
    {
		_unitType.InitSetting();
    }

    // 유닛 선택
    public void SelectUnit()
	{
		unitMarker.SetActive(true);
		gameObject.layer = 3;
	}

	// 유닛 선택 취소
	public void DeselectUnit()
	{
		unitMarker.SetActive(false);
		gameObject.layer = 7;
	}
	
	// 유닛 움직임
	public void MoveTo(Vector2 end)
	{
		_isMoved = true;
		_animator.SetBool("isMove", _isMoved);
		_nextMovePosition = new Vector3(end.x, end.y, 0f);
		_targetPos = _nextMovePosition - transform.position;
		_targetDir = new Vector2(_targetPos.x, _targetPos.y);
	}
	
	public void SaleUnit()
    {
		Destroy(gameObject);
    }

    private void FixedUpdate()
	{
		if(transform.position.z > 1.5f)
        {
			_sprite.sortingOrder = 4;
        }
        else
        {
			_sprite.sortingOrder = 0;
		}

		if (_isMoved)
		{
			RaycastHit2D _hit_Unit = Physics2D.Raycast(transform.position, _targetDir, 0.8f, layerUnit);
			RaycastHit2D _hit_Fence = Physics2D.Raycast(transform.position, _targetDir, 0.8f, layerFence);
			Debug.DrawRay(transform.position, _targetPos, Color.red);

			// 유닛 혹은 펜스를 검출
			if (_hit_Unit || _hit_Fence)
			{
				_isMoved = false;
				_animator.SetBool("isMove", _isMoved);
			}
            else
            { 
				// 움직임
				transform.position = Vector3.MoveTowards(transform.position, _nextMovePosition, Time.fixedDeltaTime * _speed);
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
				_animator.SetFloat("MoveX", _targetDir.x);
				_animator.SetFloat("MoveY", _targetDir.y);
				_animator.SetFloat("LastMoveX", transform.position.x);
				_animator.SetFloat("LastMoveY", transform.position.y);
				if (((Vector2)transform.position - (Vector2)_nextMovePosition).sqrMagnitude < 0.2f)
				{
					_isMoved = false;
					_animator.SetBool("isMove", _isMoved);
				}
			}

			_audio.Stop();
		}
        else
        {
			_animator.SetBool("isMove", _isMoved);
			_unitType.Ability();
		}
	}
}

