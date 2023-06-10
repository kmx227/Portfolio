using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatOrVoteUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _voteUI;
    [SerializeField]
    private GameObject _chatUI;
    [SerializeField]
    private StateManager _stateManager;

    public void BtnOpenChatUI()
    {
        if(!_chatUI.activeSelf && _stateManager._gameState == StateManager.State.OnChatting)
        {
            _chatUI.SetActive(true);
            _voteUI.SetActive(false); 
        }
    }

    public void BtnOpenVoteUI()
    {
        if (!_voteUI.activeSelf && _stateManager._gameState == StateManager.State.OnChatting)
        {
            _chatUI.SetActive(false);
            _voteUI.SetActive(true);
        }     
    }
}
