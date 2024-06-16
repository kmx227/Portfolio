using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using UnityEngine.UI;

public class VoteSystem : MonoBehaviour
{
    ClickProfileAction _clickProfile;

    [SerializeField]
    private List<int> _voteList;
    [SerializeField]
    private RoleManager _roleManager;
    [SerializeField]
    private StateManager _state;
    [SerializeField]
    private Emergency _emergency;
    [SerializeField]
    private GameObject _voteResult;
    [SerializeField]
    private GameObject _voteUI;
    [SerializeField]
    private GameObject _deadPlayers;

    public int _arrestUserNum;
    public int _modeCount;
    [SerializeField] private int _voteMemberCount;

    // Start is called before the first frame update
    void OnEnable()
    {
        _clickProfile = gameObject.GetComponent<ClickProfileAction>();
        _voteList = new List<int>();
        _voteMemberCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public void AddVoteList(int num)
    {
        _voteList.Add(num);
    }

    public int ReduceVoteMemberCount()
    {
        return _voteMemberCount--;
    }

    // 투표를 가장 많이 받은 순으로 리스트 정렬 -> 가장 많이 받은 사람이 두 명 이상일 시 mode => 100 으로 구분 / 한 명이면 mode => 해당 플레이어 번호
    public int CalListMode()
    {
         var _mode01 = _voteList.GroupBy(v => v).OrderByDescending(g => g.Count()).First();
        var _mode02 = _voteList.GroupBy(v => v).OrderByDescending(g => g.Count()).Skip(1).First();

        var _mode = 100;
        if(_mode01.Count().Equals(_mode02.Count()))
        {
            _mode = 100;
        }
        else
        {
            _mode = _voteList.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;
        }

        return _mode;
    }

    // Update is called once per frame
    void Update()
    {
        if (_voteList.Count == _voteMemberCount)
        {
            _arrestUserNum = CalListMode();
            StartCoroutine(Arrest());
        }
    }

    /// <summary>
    /// 한 명의 투표대상자가 결정되면 감옥행 후 진행 / 그렇지 않으면 위치 랜덤 스폰 후 다시 진행
    /// </summary>
    /// <returns></returns>
    IEnumerator Arrest()
    {
        _voteList.Clear();
        _voteResult.SetActive(true);

        if (_arrestUserNum != 100)
        {
            var _arrestUser = _roleManager.PlayerList[_arrestUserNum];
            _clickProfile.Players[_arrestUserNum].transform.GetChild(3).GetComponent<Image>().enabled = true;
            _clickProfile.Players[_arrestUserNum].transform.GetChild(1).GetComponent<Image>().enabled = false;
            _clickProfile.Players[_arrestUserNum].transform.GetChild(4).GetComponent<Button>().interactable = false;
            _arrestUser.transform.position = new Vector3(4, 1, 9);

            var _notArrestUserList = _roleManager.PlayerList.ToList();
            _notArrestUserList.RemoveAt(_arrestUserNum);

            for (int i = 0; i < _notArrestUserList.Count; i++)
            {
                _notArrestUserList[i].transform.position = _roleManager.GetPlayerPoint(i);
            }

            _voteResult.GetComponent<VoteResultUI>().Arrest(true);
            _voteResult.GetComponent<VoteResultUI>().SetArrestedUserName(_arrestUser.GetComponent<PhotonView>().Controller.NickName);
        }
        else
        {
            for (int i = 0; i < _roleManager.mixList.Count; i++)
            {
                _roleManager.mixList[i].transform.position = _roleManager.GetPlayerPoint(i);
            }

            _voteResult.GetComponent<VoteResultUI>().Arrest(false);
        }

        _voteResult.GetComponent<VoteResultUI>().OpenUI();

        yield return new WaitForSeconds(2f);

        _voteUI.SetActive(false);

        foreach(Transform _deadPlayser in _deadPlayers.transform)
        {
            Destroy(_deadPlayser.gameObject);
        }

        if (!_emergency.PermitEmergency)
        {
            _emergency.UseEmergency(false);
        }

        _state._gameState = StateManager.State.playerMoving;
    }
}
