using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSale : MonoBehaviour
{
    [SerializeField] private RTSUnitController _rtsUnitContlor;

    public void SaleBtnClick()
    {
        for (int i = 0; i < _rtsUnitContlor.LastSelectedUnitList.Count; ++i)
        {
            _rtsUnitContlor.LastSelectedUnitList[i].SaleUnit();
            _rtsUnitContlor.DeleteUnit(_rtsUnitContlor.LastSelectedUnitList[i]);
            _rtsUnitContlor.Gm.Coin += (_rtsUnitContlor.LastSelectedUnitList[i].UnitGrade + 1) * 2;
        }
    }
}
