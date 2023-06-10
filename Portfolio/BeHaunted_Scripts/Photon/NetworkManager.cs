using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _roomInput, _nickName;
    [SerializeField] private GameObject _lobby;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _roleManager;
    [SerializeField] private Text _roomNumText;
    [SerializeField] private Text[] _currentUserList;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _LoadingImg;
    [SerializeField] private GameObject _missionManager;
    [SerializeField] private GameObject _chatOrVoteUI;
    [SerializeField] private GameObject _chatPannel;
    [SerializeField] private GameObject[] _emergencyInfoUI;
    [SerializeField] private StateManager _stateManager;
    [SerializeField] private ClickProfileAction _clickProfile;
    [SerializeField] private Emergency _emergency;
    [SerializeField] private MissionOpen _mission;
    [SerializeField] private Timer _timer;
    [SerializeField] private GameObject _helper;
    [SerializeField] private GameObject _map;
    [SerializeField] private GameObject _serverLoadingUI;
    [SerializeField] private GameObject _roleIntroduce;
    [SerializeField] private GameObject _roleStudent;
    [SerializeField] private GameObject _roleGhost;
    [SerializeField] private GameObject _loginField;
    [SerializeField] private GameObject _missionListUI;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _playerMine;
    [SerializeField] private GameObject _startCam;

    [SerializeField] private int _playerCount = 0;
    private bool _isOpenPannel = false;
    private bool _isOpenHelper;
    private bool _isOpenMap;

    public bool IsOpenPannel { get => _isOpenPannel; set => _isOpenPannel = value; }
    public GameObject Player { get => _player; set => _player = value; }

    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        //Cursor.lockState = CursorLockMode.Confined;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        _lobby.SetActive(false);
        print("!!");
    }

    private void Update()
    {
        if (_stateManager._gameState != StateManager.State.playerMoving)
        {
            Cursor.lockState = CursorLockMode.Confined;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && _stateManager._gameState == StateManager.State.playerMoving)
        {
            _emergencyInfoUI[0].SetActive(false);
            _emergencyInfoUI[1].SetActive(false);

            if (!_isOpenPannel)
            {
                _chatPannel.SetActive(true);
                _isOpenPannel = true;
            }
            else
            {
                _chatPannel.SetActive(false);
                _isOpenPannel = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!_isOpenHelper)
            {
                _helper.SetActive(true);
                _isOpenHelper = true;
            }
            else
            {
                _helper.SetActive(false);
                _isOpenHelper = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if (!_isOpenMap)
            {
                _map.SetActive(true);
                _isOpenMap = true;
            }
            else
            {
                _map.SetActive(false);
                _isOpenMap = false;
            }
        }

        else if (Input.GetMouseButtonDown(0) && _player.layer != 29)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == 21 && hit.distance < 5f)
                {
                    if (!_mission.IsFinal)
                    {
                        print("시체 클릭");
                        photonView.RPC("OpenChatPannel", RpcTarget.AllViaServer,false);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == 17 && hit.distance < 2f)
                {
                    if (!hit.collider.gameObject.GetComponent<Door>().IsOpen)
                    {
                        hit.collider.gameObject.GetComponent<Door>().OpenDoor();
                        hit.collider.gameObject.GetComponent<Door>().IsOpen = true;
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<Door>().OpenDoor();
                        hit.collider.gameObject.GetComponent<Door>().IsOpen = false;
                    }
                }
                else if (hit.collider.gameObject.layer == 24 && hit.distance < 2f && _player.layer != 29)
                {
                    if (_emergency.PermitEmergency)
                    {
                        photonView.RPC("OpenChatPannel", RpcTarget.AllViaServer, true);
                    }
                }
            }
        }
    }

    [PunRPC]
    void OpenChatPannel(bool _isEmergency)
    {
        if (_isEmergency)
        {
            _emergencyInfoUI[0].SetActive(false);
            _emergencyInfoUI[1].SetActive(true);
            _emergency.UseEmergency(true);
        }
        else
        {
            _emergencyInfoUI[0].SetActive(true);
            _emergencyInfoUI[1].SetActive(false);
        }

        AudioSource.PlayClipAtPoint(SoundManager.instance.seManager.EffectSounds[8], _player.transform.position);
        for(int i=0; i< _missionManager.GetComponent<MissionOpen>().MissionObj.Count; i++)
        {
            _missionManager.GetComponent<MissionOpen>().MissionObj[i].SetActive(false);
        }

        if(_missionManager.GetComponent<MissionOpen>().student != null)
        {
            _missionManager.GetComponent<MissionOpen>().PlayerCam.SetActive(true);
            _missionManager.GetComponent<MissionOpen>().MissionCam.SetActive(false);
        }
        _chatPannel.SetActive(true);
        _isOpenPannel = true;
        _stateManager._gameState = StateManager.State.OnChatting;
        _clickProfile.InitProfile();
        _timer.Reset_Timer();
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public override void OnConnectedToMaster()
    {
        print("서버접속완료");
        _serverLoadingUI.SetActive(false);
        _loginField.SetActive(true);
    }

    public void OnDisconnect()
    {
        // 접속 종료 함수
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CreateRoom()
    {
        string _roomNum = GetNum();

        if (PhotonNetwork.IsConnected && _nickName.text != "")
        {
            PhotonNetwork.CreateRoom(_roomNum, new RoomOptions { MaxPlayers = 8 });
            PhotonNetwork.LocalPlayer.NickName = _nickName.text;
            _roomNumText.text = _roomNum;
            _currentUserList[0].text = PhotonNetwork.LocalPlayer.NickName;
            //_lobby.SetActive(true);
        }
        else if(_nickName.text == "")
        {
            StartCoroutine(FadeFiedl(_nickName));
        }
    }

    // 방 번호 랜덤 생성
    private static string GetNum()
    {
        string _num = "";

        for(int i=0; i<4; i++)
        {
            int _rand = Random.RandomRange(0, 10);
            string _randomNum = _rand.ToString();

            _num += _randomNum;
        }

        return _num;
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected && _nickName.text != "" && _roomInput.text != "")
        {
            PhotonNetwork.JoinRoom(_roomInput.text);
            PhotonNetwork.LocalPlayer.NickName = _nickName.text;
            _roomNumText.text = _roomInput.text;
        }
        else if(_nickName.text == "")
        {
            StartCoroutine(FadeFiedl(_nickName));
        }
        else if(_roomInput.text == "")
        {
            StartCoroutine(FadeFiedl(_roomInput));
        }
    }

    public void OnLeaveRoom()
    {
        // 로비 퇴장 함수
        PhotonNetwork.LeaveRoom();
    }

    public void RPCSendMessage()
    {
        photonView.RPC("SendMessage", RpcTarget.All);
    }

    IEnumerator FadeFiedl(InputField _obj)
    {
        Color _initColor = new Color32(171, 171, 171, 255);
        Color _changedColor = new Color32(171, 80, 80, 255);
        yield return new WaitForSeconds(0.2f);
        _obj.image.color = _changedColor;
        yield return new WaitForSeconds(0.2f);
        _obj.image.color = _initColor;
        yield return new WaitForSeconds(0.2f);
        _obj.image.color = _changedColor;
        yield return new WaitForSeconds(0.2f);
        _obj.image.color = _initColor;
    }

    public override void OnDisconnected(DisconnectCause _cause)
    {
        // 접속 종료 완료 콜백
        print("종료");

        SceneManager.LoadScene(0);
    }

    public override void OnCreatedRoom() => print("방 생성!");
    public override void OnJoinedRoom()
    {
        print("방 참가!");
        _lobby.SetActive(true);

        if (!PhotonNetwork.IsMasterClient)
        {
            _startButton.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        _lobby.SetActive(false);
    }
    public override void OnJoinRoomFailed(short _returnCode, string _message)
    {
        // 룸 입장 실패 콜백
        print("참가 실패");
    }


    //------------------------------------------------------->  로비 관리

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("RenewalRoom", RpcTarget.AllBuffered);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("RenewalRoom", RpcTarget.AllBuffered);
    }

    public void BtnStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetLoadImg", RpcTarget.AllBuffered, true);
            StartCoroutine(LoadingGame1());
            photonView.RPC("Loading", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void Loading()
    {
        StartCoroutine(LoadingGame2());
    }

    IEnumerator LoadingGame2()
    {
        yield return new WaitUntil (() => _roleManager.GetComponent<RoleManager>().mixList.Count == PhotonNetwork.CurrentRoom.PlayerCount && _roleManager.GetComponent<RoleManager>().mixList[_roleManager.GetComponent<RoleManager>().mixList.Count -1].layer != 0);
        //photonView.RPC("RoleIntroduce", RpcTarget.AllViaServer);
        RoleIntroduce();
        yield return new WaitForSeconds(3f);
        //photonView.RPC("SetLoadImg", RpcTarget.AllBuffered, false);
        SetLoadImg(false);
    }

    IEnumerator LoadingGame1()
    {
        yield return new WaitForSeconds(1f);
        photonView.RPC("PlayerStart", RpcTarget.AllBufferedViaServer);
        yield return new WaitForSeconds(1f);
        photonView.RPC("SetManager", RpcTarget.AllBufferedViaServer,true);
    }

    [PunRPC]
    void SetLoadImg(bool _isTrue)
    {
        if (_isTrue)
        {
            _LoadingImg.SetActive(true);

        }
        else
        {
            _LoadingImg.SetActive(false);
            _roleIntroduce.SetActive(false);
            _roleStudent.SetActive(false);
            _roleGhost.SetActive(false);
            _stateManager._gameState = StateManager.State.playerMoving;
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    [PunRPC]
    void RoleIntroduce()
    {
        _roleIntroduce.SetActive(true);
        if (_player.layer == 15)
        {
            _roleStudent.SetActive(true);
            RenderSettings.ambientLight = new Color32(10, 10, 10, 1);
        }
        if (_player.layer == 16)
        {
            _roleGhost.SetActive(true);
            RenderSettings.ambientLight = new Color32(40, 40, 40, 1);
        }
    }

    [PunRPC]
    void SetManager(bool _isTrue)
    {
        if (_isTrue)
        {
            _roleManager.SetActive(true);
            _missionManager.SetActive(true);
            _chatOrVoteUI.SetActive(true);
            _missionListUI.SetActive(true);
        }
        else
        {
            _roleManager.SetActive(false);
            _missionManager.SetActive(false);
            _chatOrVoteUI.SetActive(false);
            _missionListUI.SetActive(false);
        }
    }

    [PunRPC]
    void PlayerStart()
    {
        _canvas.SetActive(false);
        _player =  PhotonNetwork. Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
        _startCam.GetComponent<AudioListener>().enabled = false;
    }

    [PunRPC]
    void RenewalRoom()
    {
        for(int i=0; i < _currentUserList.Length; i++)
        {
            if(i < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                _currentUserList[i].text = PhotonNetwork.PlayerList[i].NickName;
            }
            else
            {
                _currentUserList[i].text = "";
            }
        }
    }

    [PunRPC]
    void LeftRoom()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            _currentUserList[i].text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    public void ReStart()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    //------------------------------------------------------->  방 정보 확인
    [ContextMenu("방 정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 번호 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원 수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대 인원 수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string _plyaerStr = "방에 있는 플레이어 목록 : ";
            for(int i=0; i<PhotonNetwork.PlayerList.Length; i++)
            {
                _plyaerStr += PhotonNetwork.PlayerList[i].NickName + ",";
            }
            print(_plyaerStr);
        }
        else
        {
            print("룸 리스트 : " + PhotonNetwork.CountOfRooms);
        }
    }
}
