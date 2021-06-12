using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인스펙터창에서 수정 가능
[System.Serializable]
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count; //생성 개수
    public Transform tfPoolParent; //부모위치
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] ObjectInfo[] objectInfo = null; //배열 설정

    public Queue<GameObject> noteQueue = new Queue<GameObject>(); //gameObject타입의 queue

    public static ObjectPool instance; //어디서든 접근 가능 공유자원
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]); //note 다른 배열 생성 가능
    }

    Queue<GameObject> InsertQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>(); //queue 생성

        //prefab생성
        for (int i = 0; i < p_objectInfo.count; i++){

            GameObject t_clone = Instantiate(p_objectInfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false); //게임 내에서는 보이지 않음

            //부모 설정
            if(p_objectInfo.tfPoolParent != null)
            {
                t_clone.transform.SetParent(p_objectInfo.tfPoolParent);
            }
            else
            {
                t_clone.transform.SetParent(this.transform); //부모가 null값이라면 이 script가 있는 것이 부모
            }

            t_queue.Enqueue(t_clone); //반복문 후 queue안에는 count 개수만큼 들어감

        }

        return t_queue;
    }

}
