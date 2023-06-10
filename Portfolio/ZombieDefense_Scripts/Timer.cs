using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text _totalTimeText; // 시간을 표시할 text
    [SerializeField] private Text _spawnTimeText;
    [SerializeField] private Text _coinText;
    [SerializeField] EnemySystem _enemySystem;
    [SerializeField] GameManager _gameManager;
    float _totalTime; // 총 시간
    float _spawnTime; // 좀비 스폰 시간(마다)
    int _round = 0;
    int _maxRound = 5;

    private void Start()
    {
        _spawnTime = _gameManager.ZombieSpawnTime + 1;
    }

    private void Update() // 바뀌는 시간을 text에 반영 해 줄 update 생명주기
    {
        _coinText.text = $"COIN : {_gameManager.Coin}";
        _totalTime += Time.deltaTime;
        _spawnTime -= Time.deltaTime;

        _totalTimeText.text = $"{((int)_totalTime / 3600)} 시간 {((int)_totalTime / 60 % 60)} 분 {((int)_totalTime % 60)} 초";
        _spawnTimeText.text = $"{((int)_spawnTime % 60)} 초 남음";

        if (_spawnTime < 0)
        {
            if(_round >= _maxRound)
            {
                _round = 5;
            }
            else
            {
                _round += 1;
            }

            _enemySystem.ZombeList = _enemySystem.SpawnUnits(_round);
            _gameManager.CurrentRound += 1;
            _spawnTime = _gameManager.ZombieSpawnTime;
        }
    }
}
