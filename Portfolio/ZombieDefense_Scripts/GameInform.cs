using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInform : MonoBehaviour
{
    List<string> _inform = new List<string>();
    private bool _isBig = true;
    private int[] _informPosY = new int[4] {160, 250, 65, 65};
    private RectTransform _rect;
    [SerializeField] GameObject _handle;
    [SerializeField] Text _text;
    [SerializeField] Scrollbar _scrollbar;

    public List<string> Inform { get => _inform; set => _inform = value; }

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void InformChange()
    {
        if (_isBig)
        {
            _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, _informPosY[2]);
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _informPosY[3]);
            _handle.SetActive(false);
            _isBig = false;
            _scrollbar.value = 0;
            _text.text = "¡â";
        }
        else
        {
            _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, _informPosY[0]);
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _informPosY[1]);
            _handle.SetActive(true);
            _isBig = true;
            _text.text = "¡ä";
            _scrollbar.value = 0;
        }
    }
}
