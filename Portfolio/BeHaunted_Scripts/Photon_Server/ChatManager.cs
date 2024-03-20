﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class ChatTexts
{
    public string userID;
    public string chatText;
    public Texture2D picture;
}

public class ChatManager : MonoBehaviourPunCallbacks 
{
    [SerializeField]
    private List<ChatTexts> _chatting;
    [SerializeField]
    private GameObject YellowArea, WhiteArea, DateArea;
    [SerializeField]
    private RectTransform ContentRect;
    [SerializeField]
    private Scrollbar scrollBar;
    [SerializeField]
    private InputField _inputField;
    [SerializeField]
    private Text _inputText;
    AreaScript LastArea;
    [SerializeField]
    private PhotonView _pv;

    private string _messageText;

    public void BtnSendMessage()
    {
        if(_inputField.text == "")
        {
            return;
        }

        _pv.RPC("SendMessage", RpcTarget.All,PhotonNetwork.NickName,_inputField.text,null);
        _pv.RPC("CreateMessageBox", RpcTarget.All);
        _inputField.text = "";
    }

    /// <summary>
    /// 메시지 박스 생성
    /// </summary>
    [PunRPC]
    public void CreateMessageBox()
    {

        int _lastChat = _chatting.Count - 1;
        if(_chatting[_lastChat].userID == PhotonNetwork.NickName)
        {
            Chat(true, _chatting[_lastChat].chatText, _chatting[_lastChat].userID, null);
        }
        else
        {
            Chat(false, _chatting[_lastChat].chatText, _chatting[_lastChat].userID, null);
        }
    }

    [PunRPC]
    public void SendMessage(string _id, string _text, Texture2D _img)
    {
        _chatting.Add(new ChatTexts() { userID = _id, chatText = _text, picture = _img });
    }

    /// <summary>
    /// 메시지 설정
    /// </summary>
    /// <param name="isSend">메시지 박스 색상 지정</param>
    /// <param name="text">메시지 내용</param>
    /// <param name="user">이름</param>
    /// <param name="picture">프로필 이미지</param>
    public void Chat(bool isSend, string text, string user, Texture2D picture) 
    {
        if (text.Trim() == "") return;

        bool isBottom = scrollBar.value <= 0.00001f;


        //보내는 사람은 노랑, 받는 사람은 흰색영역을 크게 만들고 텍스트 대입
        AreaScript Area = Instantiate(isSend ? YellowArea : WhiteArea).GetComponent<AreaScript>();
        Area.transform.SetParent(ContentRect.transform, false);
        Area.BoxRect.sizeDelta = new Vector2(600, Area.BoxRect.sizeDelta.y);
        Area.TextRect.GetComponent<Text>().text = text;
        Fit(Area.BoxRect);


        // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입 
        float X = Area.TextRect.sizeDelta.x + 42;
        float Y = Area.TextRect.sizeDelta.y;
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                Fit(Area.BoxRect);

                if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }
        else Area.BoxRect.sizeDelta = new Vector2(X, Y);


        // 현재 것에 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        Area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        Area.User = user;


        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        Area.TimeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");


        // 이전 것과 같으면 이전 시간, 꼬리 없애기
        bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
        if (isSame) LastArea.TimeText.text = "";
        Area.Tail.SetActive(!isSame);


        // 타인이 이전 것과 같으면 이미지, 이름 없애기
        if (!isSend)
        {
            Area.UserImage.gameObject.SetActive(!isSame);
            Area.UserText.gameObject.SetActive(!isSame);
            Area.UserText.text = Area.User;
            if(picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
        }

        /*
        // 이전 것과 날짜가 다르면 날짜영역 보이기
        if (LastArea != null && LastArea.Time.Substring(0, 10) != Area.Time.Substring(0, 10))
        {
            Transform CurDateArea = Instantiate(DateArea).transform;
            CurDateArea.SetParent(ContentRect.transform, false);
            CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex() - 1);

            string week = "";
            switch (t.DayOfWeek)
            {
                case DayOfWeek.Sunday: week = "일"; break;
                case DayOfWeek.Monday: week = "월"; break;
                case DayOfWeek.Tuesday: week = "화"; break;
                case DayOfWeek.Wednesday: week = "수"; break;
                case DayOfWeek.Thursday: week = "목"; break;
                case DayOfWeek.Friday: week = "금"; break;
                case DayOfWeek.Saturday: week = "토"; break;
            }
            CurDateArea.GetComponent<AreaScript>().DateText.text = t.Year + "년 " + t.Month + "월 " + t.Day + "일 " + week + "요일";
        }
        */

        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(ContentRect);
        LastArea = Area;


        // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);
    }


    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);


    void ScrollDelay() => scrollBar.value = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            BtnSendMessage();

            _inputField.ActivateInputField();
            _inputField.Select();
        }
    }
}
