using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;
using System.Collections;

public class RoleManager : MonoBehaviour
{
    [SerializeField]
    private Student studentCS;
    [SerializeField]
    private Ghost ghostCS;
    public List<GameObject> mixList = new List<GameObject>();
    [SerializeField] 
    private List<GameObject> _playerList = new List<GameObject>();
    [SerializeField]
    public List<GameObject> diePlayer = new List<GameObject>();
    [SerializeField]
    public int _currentStudentCount = 0;
    [SerializeField]
    private int _maxGhostCount;
    [SerializeField]
    public int _currentGhostCount = 0;
    [SerializeField]
    private int _maxStudentCount;
    [SerializeField]
    private List<Vector3> _playerSpawnPoints;

    [SerializeField]
    PhotonView _pv;
    [SerializeField]
    private List<int> _playerSortList;
    [SerializeField]
    bool _isTest = false; //빌드용은 지워야함

    private List<int> _playerNumList = new List<int>();

    public enum PlayerRole
    {
        Ghost,
        Student,
        Empty
    }

    public PlayerRole playerRole;

    public int StudentCount { get => _currentStudentCount; set => _currentStudentCount = value; }
    public int MaxGhostCount { get => _maxGhostCount; set => _maxGhostCount = value; }
    public List<Vector3> PlayerSpawnPoints { get => _playerSpawnPoints; set => _playerSpawnPoints = value; }
    public List<GameObject> PlayerList { get => _playerList; set => _playerList = value; }
    public int MaxStudentCount { get => _maxStudentCount; set => _maxStudentCount = value; }

    [System.Serializable]
    public class Serialization<T>
    {
        public Serialization(List<T> _target) => target = _target;
        public List<T> target;
    }

    private void Awake()
    {
        _playerSortList = new List<int>();
    }

    private void OnEnable()
    {
        print("여기까지");
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                _playerSortList.Add(player.GetComponent<PhotonView>().ControllerActorNr);
            }

            _playerNumList = _playerSortList.ToList();
            _playerSortList = GetShuffleList(_playerSortList);
            _playerSpawnPoints = GetShuffleList(_playerSpawnPoints);

            MasterSendPlayerInfo();
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount > 5)
        {
            _maxGhostCount = 2;
        }

        _maxStudentCount = PhotonNetwork.CurrentRoom.PlayerCount - _maxGhostCount;

        StartCoroutine(RandomRole());
    }

    private void OnDisable()
    {
        _playerList.Clear();
        _playerNumList.Clear();
        _playerSortList.Clear();
        mixList.Clear();
    }

    void ShuffleArray()
    {
        //System.Random _random = new System.Random();
        //_playerSpawnPoints = _playerSpawnPoints.OrderBy(x => _random.Next()).ToArray();
        StartCoroutine(RandomRole());
    }

    IEnumerator RandomRole()
    {
        yield return new WaitUntil(() => _playerSortList.Count == PhotonNetwork.CurrentRoom.PlayerCount);
        int count = _playerSortList.Count;
        var _currentPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                if (_currentPlayers[j].GetComponent<PhotonView>().ControllerActorNr == _playerSortList[i])
                {
                    _currentPlayers[j].transform.position = _playerSpawnPoints[i];
                    mixList.Add(_currentPlayers[j]);
                }
                else if(_currentPlayers[j].GetComponent<PhotonView>().ControllerActorNr == _playerNumList[i])
                {
                    _playerList.Add(_currentPlayers[j]);
                }
            }
        }

        yield return new WaitUntil(() => _playerList.Count == PhotonNetwork.CurrentRoom.PlayerCount);
        for (int i = 0; i < _maxGhostCount; i++)
        {
            if (_isTest)
            {
                mixList[i].AddComponent<Student>();
                mixList[i].layer = 15;
                _currentStudentCount++;
            }
            else
            {
                mixList[i].AddComponent<Ghost>();
                mixList[i].layer = 16;
                _currentGhostCount++;
            }
        }
        for (int i = 0; i < mixList.Count - _maxGhostCount; i++)
        {
            mixList[_maxGhostCount + i].AddComponent<Student>();
            mixList[_maxGhostCount + i].layer = 15;
            _currentStudentCount++;
        }
    }

    private List<T> GetShuffleList<T>(List<T> _list)
    {
        for (int i = _list.Count - 1; i > 0; i--)
        {
            int rand = UnityEngine.Random.Range(0, i);

            T temp = _list[i];
            _list[i] = _list[rand];
            _list[rand] = temp;
        }

        return _list;
    }

    void MasterSendPlayerInfo()
    {
        string jdataPlayerSortList = JsonUtility.ToJson(new Serialization<int>(_playerSortList));
        string jdataPlayerSpawnList = JsonUtility.ToJson(new Serialization<Vector3>(_playerSpawnPoints));
        string jdataPlayerNumList = JsonUtility.ToJson(new Serialization<int>(_playerNumList));

        _pv.RPC("OtherReceivePlayerInfoRPC", RpcTarget.Others, jdataPlayerSortList, jdataPlayerSpawnList, jdataPlayerNumList);
    }

    [PunRPC]
    void OtherReceivePlayerInfoRPC(string jdata01, string jdata02, string jdata03)
    {
        _playerSortList = JsonUtility.FromJson<Serialization<int>>(jdata01).target;
        _playerSpawnPoints = JsonUtility.FromJson<Serialization<Vector3>>(jdata02).target;
        _playerNumList = JsonUtility.FromJson<Serialization<int>>(jdata03).target;
    }
}
