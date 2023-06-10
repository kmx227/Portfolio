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

    public List<int> VoteList { get => _voteList; set => _voteList = value; }
    public int VoteMemberCount { get => _voteMemberCount; set => _voteMemberCount = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        _clickProfile = gameObject.GetComponent<ClickProfileAction>();
        _voteList = new List<int>();
        _voteMemberCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

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
                _notArrestUserList[i].transform.position = _roleManager.PlayerSpawnPoints[i];
            }

            _voteResult.GetComponent<VoteResultUI>().IsArrest = true;
            _voteResult.GetComponent<VoteResultUI>().ArrestUserName = _arrestUser.GetComponent<PhotonView>().Controller.NickName;
        }
        else
        {
            for (int i = 0; i < _roleManager.mixList.Count; i++)
            {
                _roleManager.mixList[i].transform.position = _roleManager.PlayerSpawnPoints[i];
            }

            _voteResult.GetComponent<VoteResultUI>().IsArrest = false;
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
