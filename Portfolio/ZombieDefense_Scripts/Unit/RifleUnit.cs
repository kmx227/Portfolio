using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleUnit : Unit
{
    [SerializeField] private int _unitGrade = 0;
    [SerializeField] private bool _isAttack = false; // **
    [SerializeField] private float _attackRange = 3f; // **
    [SerializeField] private float _attackDamage = 10; // **
    [SerializeField] private float _count = 0f; // ** <- 직렬화 해제 예정

    [SerializeField] private Animator _animator;
    [SerializeField] CheckAndAttack _abilityControler;

    public Animator Animator { get => _animator; set => _animator = value; }

    public override int CalUnitGrade() => base.CalUnitGrade();

    public override void InitSetting()
    {
        unitData.unitGrade = CalUnitGrade();
        _unitGrade = unitData.unitGrade;
        SetValue(_unitGrade);
        _animator.SetInteger("UnitGrade", _unitGrade);
        unitData.isAttack = _isAttack;
        unitData.attackDamage = _attackDamage;
        unitData.attackRange = _attackRange;
        unitData.count = _count;
    }

    //public override GameObject CheckToZombie(float _range) => base.CheckToZombie(_range);

    public override void Ability()
    {
        _abilityControler.Ability(unitData.attackDamage, unitData.attackRange);
    }
}
