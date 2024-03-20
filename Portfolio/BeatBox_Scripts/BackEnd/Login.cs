﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd; //뒤끝서버

public class Login : MonoBehaviour
{
    [SerializeField] GameObject goStageUI = null;
    [SerializeField] InputField id = null;
    [SerializeField] InputField pw = null;

    DataBase_Manager theDatabase;

    // Start is called before the first frame update
    void Start()
    {
        theDatabase = FindObjectOfType<DataBase_Manager>();
        Backend.InitializeAsync(true, InitializeCallback =>
         {
             if (InitializeCallback.IsSuccess())
             {
                 Debug.Log(Backend.Utils.GetServerTime());
                 Debug.Log(Backend.Utils.GetGoogleHash()); //안드로이드 연동키
             }
             else
             {
                 Debug.Log("초기화 실패(인터넷 문제 등등)");
             }
         });
    }

    private void Update()
    {
        Backend.AsyncPoll();
    }

    public void BtnRegist()
    {
        string t_id = id.text;
        string t_pw = pw.text;

        BackendReturnObject bro = Backend.BMember.CustomSignUp(t_id, t_pw, "Test");

        if (bro.IsSuccess())
        {
            Debug.Log("회원가입 완료");
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("회원가입 실패");
        }
    }

    public void BtnLogin()
    {
        string t_id = id.text;
        string t_pw = pw.text;

        BackendReturnObject bro = Backend.BMember.CustomLogin(t_id, t_pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인 완료");
            theDatabase.LoadScore();
            this.gameObject.SetActive(false);
            goStageUI.SetActive(true);
        }
        else
        {
            Debug.Log("로그인 실패");
        }
    }

}
