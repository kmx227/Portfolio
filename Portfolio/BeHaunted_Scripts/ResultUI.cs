using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ResultUI : MonoBehaviour
{
    [SerializeField] RoleManager _roleManager;
    [SerializeField] private Text _winner;
    [SerializeField] private Text[] _students;
    [SerializeField] private Text[] _ghosts;

    private bool _studentWin = false;

    public bool StudentWin { get => _studentWin; set => _studentWin = value; }

    // Start is called before the first frame update
    private void OnEnable()
    {
        for (int i = 0; i < _roleManager.mixList.Count; i++)
        {
            if (i < _roleManager.MaxGhostCount)
            {
                _ghosts[i].text = _roleManager.mixList[i].GetComponent<PhotonView>().Controller.NickName;
            }
            else
            {
                _students[i - _roleManager.MaxGhostCount].text = _roleManager.mixList[i].GetComponent<PhotonView>().Controller.NickName;
            }
        }

        if (_studentWin)
        {
            _winner.text = "ÇÐ»ý ½Â¸®";
        }
        else
        {
            _winner.text = "±Í½Å ½Â¸®";
        }
    }
}
