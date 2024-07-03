using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    [SerializeField] RouletteData rouletteData;
    [SerializeField] GameObject button;
    [SerializeField] GameObject rouletteUI;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject upgradeUI;
    [SerializeField] GameObject buttonUI;

    private Sprite sprite;
    private Text text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        if(rouletteData.ID < 2)
        {
            if (GameManager.instance.GetLevel(rouletteData.ID) > 4)
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            if (GameManager.instance.GetLevel(rouletteData.ID) > 3)
            {
                button.GetComponent<Button>().interactable = false;
            }
        }

        button.GetComponent<Image>().sprite = rouletteData.sprite;
        text.text = rouletteData.text;
    }

    public void OpenRouletteUI()
    {
        rouletteUI.SetActive(true);
        rouletteUI.GetComponent<RoulettePlayer>().Init(rouletteData);
        panel.gameObject.SetActive(false);
    }

    public void CloseRouletteUI()
    {
        upgradeUI.gameObject.SetActive(false);
        buttonUI.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
    }
}
