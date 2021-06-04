using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    [SerializeField] GameObject[] stageArray = null;
    GameObject currentStage;
    Transform[] stagePlates;
    //Transform[] fallStagePlates;

    //타일생성때 밑에서 올라오게 만드는거
    [SerializeField] float offsetY = 3;
    [SerializeField] float plateSpeed = 10;

    public int stepCount = 0;
    int totalPlateCount = 0;

    // Start is called before the first frame update

    public void RemoveStage()
    {
        if(currentStage != null)
        {
            Destroy(currentStage);
        }
    }
    public void SettingStage(int p_songNum)
    {
        stepCount = 0;
        currentStage = Instantiate(stageArray[p_songNum], Vector3.zero, Quaternion.identity);

        stagePlates = currentStage.GetComponent<Stage>().plates;
        totalPlateCount = stagePlates.Length;

        for (int i = 0; i < totalPlateCount; i++)
        {
            
            stagePlates[i].position = new Vector3(stagePlates[i].position.x, 
                                                  stagePlates[i].position.y + offsetY, //이부분이 -3으로 밑으로 내려가게 하는거
                                                  stagePlates[i].position.z);
        }
    }

    public void ShowNextPlate() {
        if (stepCount < totalPlateCount)
        {
            StartCoroutine(MovePlateCo(stepCount++));
        }
    }

    //코루틴 타일의 움직임 부드럽게
    IEnumerator MovePlateCo(int p_num)
    {
        stagePlates[p_num].gameObject.SetActive(true);

        // 타일 올라옴
        Vector3 t_destPos = new Vector3(stagePlates[p_num].position.x,
                                                  stagePlates[p_num].position.y - offsetY, //원위치되게
                                                  stagePlates[p_num].position.z);

        // 올라오는 타일의 효과
        while(Vector3.SqrMagnitude(stagePlates[p_num].position - t_destPos) >= 0.001f)
        {
            stagePlates[p_num].position = Vector3.Lerp(stagePlates[p_num].position, t_destPos, plateSpeed * Time.deltaTime);
            yield return null;
        }
        stagePlates[p_num].position = t_destPos;
    }
}
