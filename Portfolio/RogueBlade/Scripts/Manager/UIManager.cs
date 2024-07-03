using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum UIType { Exp, Phaze, Coin, Health }
    public UIType Type;

    private Text text;
    private Slider slider;
    [SerializeField] private Image[] images;


    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (Type)
        {
            case UIType.Exp:
                var curExp = GameManager.instance.GetCurrentExp();
                slider.value = (float)curExp / 100f;
                break;
            case UIType.Phaze:
                var curProgress = GameManager.instance.GetCurrentPhaseProgress();
                slider.value = curProgress / 100f;
                break;
            case UIType.Coin:
                text.text = GameManager.instance.GetCurrentCoin().ToString();
                break;
            case UIType.Health:
                for(int i = 0; i < 3; i++)
                {
                    if(i < GameManager.instance.GetGameHP())
                    {
                        images[i].enabled = true;
                    }
                    else
                    {
                        images[i].enabled = false;
                    }
                }
                break; 

        }
    }
}
