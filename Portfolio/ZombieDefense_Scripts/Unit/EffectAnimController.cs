using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectAnimController : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] Text _text;
    [SerializeField] GameObject _effect;
    private bool _clickOff = false;
    private int _unitNum;
    private int _unitGrade;
    private string _unitName;

    public int UnitNum { get => _unitNum; set => _unitNum = value; }
    public int UnitGrade { get => _unitGrade; set => _unitGrade = value; }
    public string UnitName { get => _unitName; set => _unitName = value; }

    private void OnEnable()
    {
        StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        _anim.SetInteger("Unit", _unitNum);
        _anim.SetInteger("UnitGrade", _unitGrade);
        var name = _unitName;
        _text.text = "! " + name + " µÓ¿Â !";
        yield return new WaitForSeconds(0.5f);
        _anim.SetTrigger("Shoot");
        _anim.ResetTrigger("Reload");
        yield return new WaitForSeconds(0.5f);
        _anim.ResetTrigger("Shoot");
        _anim.SetTrigger("Reload");
        yield return new WaitForSeconds(0.5f);
        _clickOff = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_clickOff)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _effect.SetActive(false);
            }
        }
    }
}
