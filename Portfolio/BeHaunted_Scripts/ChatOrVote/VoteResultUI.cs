using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteResultUI : MonoBehaviour
{
    [SerializeField] GameObject _pannel;
    [SerializeField] GameObject[] _voteInfo;

    private bool _isArrest = false;
    private string _arrestUserName;

    public bool IsArrest { get => _isArrest; set => _isArrest = value; }
    public string ArrestUserName { get => _arrestUserName; set => _arrestUserName = value; }

    // Start is called before the first frame update
    public void OpenUI()
    {
        StartCoroutine(CloseUI());

        if (_isArrest)
        {
            _voteInfo[0].SetActive(true);
            _voteInfo[0].transform.GetChild(2).GetComponent<Text>().text = _arrestUserName;
            _voteInfo[1].SetActive(false);
        }
        else
        {
            _voteInfo[1].SetActive(true);
            _voteInfo[0].SetActive(false);
        }
    }

    IEnumerator CloseUI()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        _pannel.SetActive(false);
    }
}
