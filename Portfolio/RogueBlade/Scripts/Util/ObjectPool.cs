using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    private GameObject[] _poolingObjectPrefabs;

    private Queue<GameObject> _poolingBlockQueue = new Queue<GameObject>();
    private Queue<GameObject> _poolingElectricBlockQueue = new Queue<GameObject>();
    private Queue<GameObject> _poolingBombQueue = new Queue<GameObject>();
    private Queue<GameObject> _poolingBulletQueue = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        Initialize(20);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            _poolingBlockQueue.Enqueue(CreateNewObject(0));
            _poolingBulletQueue.Enqueue(CreateNewObject(3));
        }

        for(int i=0; i<3; i++)
        {
            _poolingElectricBlockQueue.Enqueue(CreateNewObject(1));
            _poolingBombQueue.Enqueue(CreateNewObject(2));
        }
    }

    private GameObject CreateNewObject(int num)
    {
        var _objPrefab = _poolingObjectPrefabs[num];
        var _newObj = Instantiate(_objPrefab);
        _newObj.SetActive(false);
        _newObj.transform.SetParent(transform);
        return _newObj;
    }

    public static GameObject GetObject(int num)
    {/*
        if (num == 0)
        {
            return ObjectOfQueue(Instance._poolingBlockQueue, num);
        }
        else if (num == 1)
        {
            return ObjectOfQueue(Instance._poolingElectricBlockQueue, num);
        }
        else if (num == 2)
        {
            return ObjectOfQueue(Instance._poolingBombQueue, num);
        }
        else
        {
            return ObjectOfQueue(Instance._poolingBulletQueue, num);
        }*/
        
        switch (num)
        {
            case 0:
                return ObjectOfQueue(Instance._poolingBlockQueue, num);
            case 1:
                return ObjectOfQueue(Instance._poolingElectricBlockQueue, num);
            case 2:
                return ObjectOfQueue(Instance._poolingBombQueue, num);
            default:
                return ObjectOfQueue(Instance._poolingBulletQueue, num);
        }
    }

    private static GameObject ObjectOfQueue(Queue<GameObject> queue, int num)
    {
        if (queue.Count > 0)
        {
            var _obj = queue.Dequeue();
            _obj.transform.SetParent(null);
            _obj.gameObject.SetActive(true);
            return _obj;
        }
        else
        {
            var _newObj = Instance.CreateNewObject(num);
            _newObj.gameObject.SetActive(true);
            _newObj.transform.SetParent(null);
            return _newObj;
        }
    }

    public static void ReturnObject(GameObject _obj, int num)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(Instance.transform);

        switch (num)
        {
            case 0:
                Instance._poolingBlockQueue.Enqueue(_obj);
                break;
            case 1:
                Instance._poolingElectricBlockQueue.Enqueue(_obj);
                break;
            case 2:
                Instance._poolingBombQueue.Enqueue(_obj);
                break;
            default:
                Instance._poolingBulletQueue.Enqueue(_obj);
                break;
        }
    }
}
