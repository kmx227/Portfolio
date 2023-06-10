using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum UnitTypes
{
	Rifle,
	Sniper,
	Chemical,
	Engineer
}

public struct UnitData
{
	public int unitGrade; // **
	public bool isAttack; // **
	public float attackRange; // **
	public GameObject nearestZomibe;  // **
	public float attackDamage; // **
	public float count; // ** <- 직렬화 해제 예정
}

public abstract class Unit : MonoBehaviour
{

	private float[] _probs = new float[6] { 50f, 35f, 10f, 4f, 0.8f, 0.2f };
	[SerializeField]
	private UnitType _type;
	[SerializeField] private bool _isMoved = false; // **
	[SerializeField] private float _speed; // **
	public UnitData unitData;
	public abstract void InitSetting();
	public abstract void Ability();
	[SerializeField] private GameObject _nearestZomibe;  // **

	private GameManager _gm;

	public int UnitGrade { get => unitData.unitGrade; set => unitData.unitGrade = value; }
	public UnitType Type { get => _type; }

	public IUnitAbility _iUnitAbility;

	public void SetIUnitAbility(IUnitAbility newAttackable)
	{
		_iUnitAbility = newAttackable;
	}

	public virtual int CalUnitGrade()
	{
		float _totalProbs = 100f;
		float _randomValue = Random.value * _totalProbs;

		for (int i = 0; i < _probs.Length; i++)
		{
			if (_randomValue < _probs[i])
			{
				return i;
			}
			else
			{
				_randomValue -= _probs[i];
			}
		}

		return _probs.Length - 1;
	}

	public void SetValue(int _grade)
	{
		unitData.unitGrade = _grade;
		switch (unitData.unitGrade)
		{
			case 0:
				unitData.attackRange = 3f;
				unitData.attackDamage = 25;
				break;
			case 1:
				unitData.attackRange = 5f;
				unitData.attackDamage = 50;
				break;
			case 2:
				unitData.attackRange = 3f;
				unitData.attackDamage = 10;
				break;
			case 3:
				unitData.attackRange = 3f;
				unitData.attackDamage = 10;
				break;
			case 4:
				unitData.attackRange = 3f;
				unitData.attackDamage = 10;
				break;
			case 5:
				unitData.attackRange = 3f;
				unitData.attackDamage = 10;
				break;
		}
	}
}