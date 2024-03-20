using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd; //뒤끝 서버
using LitJson;

public class DataBase_Manager : MonoBehaviour
{
    public int[] score;
    Where where = new Where();

    public void SaveScore()
    {
        BackendReturnObject bro = Backend.GameData.Get("Score", where);

        Param data1 = new Param(); //Param -> 뒤끝서버에서 쓰는 데이터 저장 클래스
        data1.Add("Scores", score); //add로 추가

        if (bro.Rows().Count > 0)
        {
            //데이터 업데이트
            string inDate = bro.Rows()[0]["inDate"]["S"].ToString();
            Backend.GameData.Update("Score", inDate, data1, (callback) =>
            {
                Debug.Log(inDate);

            });
        }
        else
        {
            //데이터 삽입
            Backend.GameData.Insert("Score", data1, (callback) =>
            {
                Debug.Log("정보저장 성공");
            });
        }
    }

    public void LoadScore()
    {
        var bro = Backend.GameData.Get("Score", where);     
        JsonData t_data = bro.GetReturnValuetoJSON();

        if (bro.Rows().Count > 0)
        {
            JsonData t_List = t_data["rows"][0]["Scores"]["L"]; //score를 list형태로 가져옴
            for (int i = 0; i < t_List.Count; i++)
            {
                var t_value = t_List[i]["N"]; //num타입으로 가져옴
                score[i] = int.Parse(t_value.ToString()); //integer형식
            }
            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("로드 할 것 없음");
        }
    }
}
