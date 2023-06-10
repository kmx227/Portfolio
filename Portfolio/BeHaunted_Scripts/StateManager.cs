using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public enum State
    {
        WaitingForGame,
        playerMoving,
        IsPaused,
        OnMiniGame,
        OnChatting,
        IsDead
    }

    public State _gameState = new State();
}
