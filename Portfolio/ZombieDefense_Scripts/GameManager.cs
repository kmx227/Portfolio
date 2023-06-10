using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // ** 직렬화 해제 목록
    [SerializeField] private int _currentRound = 1;
    [SerializeField] private int _currentZombieCount; // **
    [SerializeField] private int _maxZombieCount = 300; // **
    [SerializeField] private float _zombieSpawnTime; // **
    [SerializeField] private float _fenceHp = 1000f; // **
    [SerializeField] private int _coin = 0; // **
    [SerializeField] private Slider _fenceHpSlider;
    [SerializeField] private GameInform _inform;
    [SerializeField] private GameObject _walls;
    [SerializeField] private GameObject _ending;

    private bool _isEnding = false;
    private AudioSource _audio;

    public float ZombieSpawnTime { get => _zombieSpawnTime; set => _zombieSpawnTime = value; }
    public int Coin { get => _coin; set => _coin = value; }
    public float FenceHp { get => _fenceHp; set => _fenceHp = value; }
    public Slider FenceHpSlider { get => _fenceHpSlider; set => _fenceHpSlider = value; }
    public int CurrentRound { get => _currentRound; set => _currentRound = value; }

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentZombieCount = transform.GetChild(0).GetComponent<EnemySystem>().ZombeList.Count;
        if(_maxZombieCount < _currentZombieCount || _fenceHp <= 0f && !_isEnding)
        {
            _isEnding = true;
            Time.timeScale = 0f;
            StartCoroutine(Ending());
        }
    }

    IEnumerator Ending()
    {
        _inform.Inform.Add("<color=red> GAME OVER.. </color>");
        _walls.SetActive(false);
        _ending.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _audio.Play();
    }
}
