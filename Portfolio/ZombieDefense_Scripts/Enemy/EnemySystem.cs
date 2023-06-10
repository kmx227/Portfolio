using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    List<Zombie> _zombeList = new List<Zombie>();
    [SerializeField] List<GameObject> _zombieSpawnPoints = new List<GameObject>();
	[SerializeField] GameObject _zombieListObj; // 스폰된 좀비가 포함된 부모 객체

	private AudioSource _audio;
	[SerializeField] private AudioClip[] _audioClip;

    public static EnemySystem Instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;

    Queue<Zombie> poolingObjectQueue = new Queue<Zombie>();

    public List<Zombie> ZombeList { get => _zombeList; set => _zombeList = value; }

    private void Awake()
    {
		_audio = GetComponent<AudioSource>();
        Instance = this;

        Initialize(150);
    }

    // Start is called before the first frame update
    public List<Zombie> SpawnUnits(int _round)
	{
		var _currentRound = transform.GetComponentInParent<GameManager>().CurrentRound;
		var _randomSound = Random.Range(0, 3);
		_audio.clip = _audioClip[_randomSound];
		_audio.Play();

		for(int i=0; i<30; i++)
        {
			var _randomNum = Random.Range(0, 20);
			Vector3 position = _zombieSpawnPoints[_randomNum].transform.position;
			position = new Vector3(Random.Range(position.x - 10, position.x + 10), Random.Range(position.y - 10, position.y + 10), 0);

            //GameObject clone = Instantiate(GetComponent<RoundZombieList>().ZombieRoundList[_round], position, Quaternion.identity);
            //Zombie unit = clone.GetComponent<Zombie>();

            Zombie unit = GetObject(_round);
            unit.transform.position = position;
			//unit.transform.SetParent(_zombieListObj.transform);

			_zombeList.Add(unit);
		}

		return _zombeList;
	}

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private Zombie CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<Zombie>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(_zombieListObj.transform);
        return newObj;
    }

    public Zombie GetObject(int _round)
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            //obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            obj.GetComponent<SpriteRenderer>().sprite = GetComponent<RoundZombieList>().ZombieRoundList[_round].GetComponent<SpriteRenderer>().sprite;
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            //newObj.transform.SetParent(null);
            newObj.GetComponent<SpriteRenderer>().sprite = GetComponent<RoundZombieList>().ZombieRoundList[_round].GetComponent<SpriteRenderer>().sprite;
            return newObj;
        }
    }

    public void ReturnObject(Zombie obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_zombieListObj.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
