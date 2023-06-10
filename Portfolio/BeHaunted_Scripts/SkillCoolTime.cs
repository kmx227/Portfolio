using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTime : MonoBehaviour
{
    [SerializeField] Button[] _skiilButton;
    [SerializeField] Image[] _coolTimeImage;
    [SerializeField] float[] _skiilCoolTime;

    [SerializeField] private bool[] _OnCoolTime = new bool[3];

    public bool[] OnCoolTime { get => _OnCoolTime; set => _OnCoolTime = value; }
    public Image[] CoolTimeImage { get => _coolTimeImage; set => _coolTimeImage = value; }

    public void ClickSkill(int _skillNum)
    {
        if(_skillNum != 0)
        {
            _coolTimeImage[_skillNum].fillAmount = 0;
        }

        _OnCoolTime[_skillNum] = true;

        StartCoroutine(StartCoolTime(_skillNum));
    }

    public void ClickOnRunButton()
    {
        _coolTimeImage[0].fillAmount -= Time.deltaTime * (1 / _skiilCoolTime[0]);
    }

    IEnumerator StartCoolTime(int _index)
    {
        if(_index == 0)
        {
            while (_coolTimeImage[_index].fillAmount != 1)
            {
                _coolTimeImage[_index].fillAmount += Time.deltaTime * (1 / _skiilCoolTime[2]);

                yield return new WaitUntil(() => _OnCoolTime[0] == true);
            }

        }
        else
        {
            while (_coolTimeImage[_index].fillAmount != 1)
            {
                _coolTimeImage[_index].fillAmount += Time.deltaTime * (1 / _skiilCoolTime[_index]);

                yield return new WaitForEndOfFrame();
            }

            _OnCoolTime[_index] = false;
            yield return null;
        }
    }
}
