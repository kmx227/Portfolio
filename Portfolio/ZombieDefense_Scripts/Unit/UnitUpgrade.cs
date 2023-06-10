using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgrade : MonoBehaviour
{
    [SerializeField] private Text[] _upgradeCounts;
    [SerializeField] private Button[] _upgradeButtons;
    private int[] _upgradeDamage;

    [SerializeField] Text _probShowText;
    [SerializeField] GameObject _probWindow;
    [SerializeField] GameObject _upgradeWindow;

    private RTSUnitController _unitCtr;

    public int[] UpgradeDamage { get => _upgradeDamage; set => _upgradeDamage = value; }

    //private bool[] _acitveUI;

    // Start is called before the first frame update
    void Start()
    {
        _unitCtr = GetComponentInChildren<RTSUnitController>();
        _upgradeDamage = new int[4] {1, 1, 1, 1 };
        //_acitveUI = new bool[2] { false, false };

        for(int i=0; i<_upgradeButtons.Length; i++)
        {
            var _num = i;
            _upgradeButtons[_num].onClick.AddListener(() => Upgrade(_num));
        }
    }

    private void Upgrade(int _unitNum)
    {
        if(GetComponent<GameManager>().Coin >= 10)
        {
            GetComponent<GameManager>().Coin -= 10;

            _upgradeDamage[_unitNum] += 1;
            _upgradeCounts[_unitNum].text = _upgradeDamage[_unitNum].ToString();
            foreach (UnitController _unit in _unitCtr.UnitList)
            {
                if ((int)_unit.Type == _unitNum)
                {
                    _unit.AttackDamage += 1;
                }
            }
        }
    }

    public void ActiveProbWindow()
    {
        if(_probWindow.activeSelf == false)
        {
            _probShowText.text = "È®·ü ²ô±â";
            _probWindow.SetActive(true);
            //_acitveUI[0] = true;
        }
        else
        {
            _probShowText.text = "È®·ü º¸±â";
            _probWindow.SetActive(false);
            //_acitveUI[0] = false;
        }
    }

    public void ActiveUpgradeWindow()
    {
        if (_upgradeWindow.activeSelf == false)
        {
            _upgradeWindow.SetActive(true);
            //_acitveUI[1] = true;
        }
        else
        {
            _upgradeWindow.SetActive(false);
            //_acitveUI[1] = false;
        }
    }
}
