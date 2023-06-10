using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UserInfoOnVoteUI : MonoBehaviour
{
    [SerializeField]
    private int _userNumInfo;
    [SerializeField]
    private Text _userId;
    [SerializeField]
    private RoleManager _roleManager;

    public int UserNumInfo { get => _userNumInfo; }

    private void OnEnable()
    {
        for (int i = 0; i < _roleManager.PlayerList.Count; i++)
        {
            if (_roleManager.PlayerList[i].layer == 29)
            {
                if(_userNumInfo == i)
                {
                    transform.GetChild(2).GetComponent<Image>().enabled = true;
                    transform.GetChild(4).GetComponent<Button>().interactable = false;
                }
            }
        }

        if (_userNumInfo < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            _userId.text = _roleManager.PlayerList[_userNumInfo].GetComponent<PhotonView>().Controller.NickName;
        }
        else
        {
            this.gameObject.SetActive(false);
            return;
        }
    }
}
