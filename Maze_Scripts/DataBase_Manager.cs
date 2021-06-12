using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class DataBase_Manager : MonoBehaviour
{
    public int[] score;

    public void SaveScore()
    {
        //비동기 통신(백그라운드 작업)
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "Score", UserDataBro => //userDataBro는 임의로 이름설정
        {
            if (UserDataBro.IsSuccess())
            {
                Param data = new Param(); //Param -> 뒤끝서버에서 쓰는 데이터 저장 클래스
                data.Add("Scores", score); //add로 추가

                if (UserDataBro.GetReturnValuetoJSON()["rows"].Count > 0) //데이터 개수 확인/ 데이터가 있는 경우
                {
                    string t_Indate = UserDataBro.GetReturnValuetoJSON()["rows"][0]["inData"]["S"].ToString();
                    BackendAsyncClass.BackendAsync(Backend.GameInfo.Update, "Score", t_Indate, data, (t_callback) =>
                    {

                    });
                }
                else
                {
                    BackendAsyncClass.BackendAsync(Backend.GameInfo.Insert, "Score", data, (t_callback) =>
                    {

                    });
                }
            }
        });
    }

    public void LoadScore()
    {
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "Score", UserDataBro =>
         {
             JsonData t_data = UserDataBro.GetReturnValuetoJSON();

             if (t_data.Count > 0)
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
                 Debug.Log("로드할 것 없음");
             }
         });
    }
}
