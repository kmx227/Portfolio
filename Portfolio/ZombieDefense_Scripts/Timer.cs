using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text _totalTimeText; // �ð��� ǥ���� text
    [SerializeField] private Text _spawnTimeText;
    [SerializeField] private Text _coinText;
    [SerializeField] EnemySystem _enemySystem;
    [SerializeField] GameManager _gameManager;
    float _totalTime; // �� �ð�
    float _spawnTime; // ���� ���� �ð�(����)
    int _round = 0;
    int _maxRound = 5;

    private void Start()
    {
        _spawnTime = _gameManager.ZombieSpawnTime + 1;
    }

    private void Update() // �ٲ�� �ð��� text�� �ݿ� �� �� update �����ֱ�
    {
        _coinText.text = $"COIN : {_gameManager.Coin}";
        _totalTime += Time.deltaTime;
        _spawnTime -= Time.deltaTime;

        _totalTimeText.text = $"{((int)_totalTime / 3600)} �ð� {((int)_totalTime / 60 % 60)} �� {((int)_totalTime % 60)} ��";
        _spawnTimeText.text = $"{((int)_spawnTime % 60)} �� ����";

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
