using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
	[SerializeField]
	private	GameObject[] unitPrefab;
	private float[] _probs = new float[6] { 50f, 35f, 10f, 4f, 0.8f, 0.2f };
	[SerializeField] private GameObject _unitListObj; // 스폰 된 유닛들이 포함 된 부모 객체
	[SerializeField] GameInform _infom;
	[SerializeField] EffectAnimController _effectAnim;
	[SerializeField] GameObject _effect;
	/*[SerializeField]
	private	int			maxUnitCount;*/

	public int exampeRandomUnit;

	List<UnitController> unitList = new List<UnitController>(100);

	private	Vector2		minSize = new Vector2(-0.5f, -0.5f);
	private	Vector2		maxSize = new Vector2(0.5f, 0.5f);

	// 유닛 생성                   ** 추후 랜덤 생성으로 변경 예정 'unitPrefab' 부분이 랜덤으로 변경
    public List<UnitController> SpawnUnits()
	{
		Vector3 position = new Vector3(Random.Range(minSize.x, maxSize.x), Random.Range(minSize.y, maxSize.y),0);
		position = new Vector3(position.x, position.y, position.y);

		var _random = Random.Range(0, 4);
		GameObject		clone	= Instantiate(unitPrefab[_random], position, Quaternion.identity);
		UnitController	unit	= clone.GetComponent<UnitController>();

		_effectAnim.UnitNum = _random;
		unit.transform.SetParent(_unitListObj.transform);

		unit.AttackDamage += (transform.GetComponentInParent<UnitUpgrade>().UpgradeDamage[(int)unit.Type] - 1);

		unitList.Add(unit);
		StartCoroutine(UnitInfrom(unit));
	
		return unitList;
	}

	IEnumerator UnitInfrom(UnitController unit)
    {
		yield return new WaitForEndOfFrame();

		var _unitName = unit.UnitName;
		var _gradeColor = "<color=red>레드</color>";

		switch (unit.GetComponent<Unit>().UnitGrade)
        {
			case 0:
				_gradeColor = "<color=black>일반</color>";
				break;
			case 1:
				_gradeColor = "<color=green>고급</color>";
				break;
			case 2:
				_gradeColor = "<color=blue>희귀</color>";
				break;
			case 3:
				_gradeColor = "<color=purple>영웅</color>";
				break;
			case 4:
				_gradeColor = "<color=orange>전설</color>";
				break;
			case 5:
				_gradeColor = "<color=red>신화</color>";
				break;
		}

		var _text = _unitName + " / " + _gradeColor;
		_effectAnim.UnitName = _text;
		_effectAnim.UnitGrade = unit.GetComponent<Unit>().UnitGrade;
		if(unit.GetComponent<Unit>().UnitGrade >= 3)
        {
			_effect.SetActive(true);
        }

		_infom.Inform.Add(_text);
	}

	public List<UnitController> SpawnUnit(GameObject _obj)
    {
		Vector3 position = new Vector3(Random.Range(minSize.x, maxSize.x), Random.Range(minSize.y, maxSize.y), 0);
		position = new Vector3(position.x, position.y, position.y);

		GameObject clone = Instantiate(_obj, position, Quaternion.identity);
		UnitController unit = clone.GetComponent<UnitController>();

		unit.transform.SetParent(_unitListObj.transform);
		unitList.Add(unit);
		StartCoroutine(UnitInfrom(unit));

		return unitList;
    }

    private int CalUnitGrade()
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
}

