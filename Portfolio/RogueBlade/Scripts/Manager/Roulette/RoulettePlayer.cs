using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoulettePlayer : MonoBehaviour
{
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject rouletteSkillObject;
    [SerializeField] private Button insertCoinButton;

    private static RouletteData rouletteData;
    private bool hasPet;
    private int ItemCnt = 3;
    private List<float> probs = new List<float>();
    private int answer;

    // ·ê·¿ °ª ÃÊ±âÈ­
    public void Init(RouletteData data)
    {
        probs.Clear();
        rouletteData = data;
        if(rouletteData.percent > ((1 - rouletteData.percent) / 2)) // ·ê·¿ ¸®½ºÆ®¸¦ È®·üÀÌ ³ôÀº ¼øÀ¸·Î ¼³Á¤
        {
            probs.Add(rouletteData.percent);
            probs.Add((1 - rouletteData.percent) / 2);
            probs.Add((1 - rouletteData.percent) / 2);
        }
        else
        {
            probs.Add((1 - rouletteData.percent) / 2);
            probs.Add((1 - rouletteData.percent) / 2);
            probs.Add(rouletteData.percent);
        }
        insertCoinButton.interactable = true;
        rouletteSkillObject.transform.localPosition = new Vector3(0, 1500f, 0);
    }

    // ·ê·¿ ½ÇÇà
    IEnumerator StartRoulette()
    {
        rouletteSkillObject.transform.localPosition = new Vector3(0, 1500f, 0);
        yield return new WaitForSecondsRealtime(0.5f);
        answer = Choose(probs);
        for (int i = 0; i < (ItemCnt * (6 * 4) + answer * 2); i++)
        {
            rouletteSkillObject.transform.localPosition -= new Vector3(0, 250f, 0);
            if (rouletteSkillObject.transform.localPosition.y <= 0f)
            {
                rouletteSkillObject.transform.localPosition += new Vector3(0, 1500f, 0);
            }
            yield return new WaitForSecondsRealtime(0.02f);
        }

        print(answer);
        if (answer == 0) // ¼º°ø -> ÇýÅÃ
        {
            switch (rouletteData.ID)
            {
                case 0:
                    if ((GameManager.instance.GetCurrentShieldLevel()) < 5)
                    {
                        GameManager.instance.Upgrade(rouletteData.ID, rouletteData.power[GameManager.instance.GetCurrentShieldLevel()]);
                        if ((GameManager.instance.GetCurrentShieldLevel()) == 5)
                            insertCoinButton.interactable = false;
                    }
                    break;
                case 1:
                    if ((GameManager.instance.GetCurrentAttackLevel()) < 5)
                    {
                        GameManager.instance.Upgrade(rouletteData.ID, rouletteData.power[GameManager.instance.GetCurrentAttackLevel()]);
                        if ((GameManager.instance.GetCurrentAttackLevel()) == 5)
                            insertCoinButton.interactable = false;
                    }
                    break;
                case 2:
                    if (GameManager.instance.GetLevel(2) < 4)
                    {
                        var randNum = Random.Range(0, hasPet ? rouletteData.weapons.Count : rouletteData.weapons.Count + 1);
                        if (randNum == rouletteData.weapons.Count)
                        {
                            hasPet = true;
                            GameManager.instance.SpawnRiflePet();
                            GameManager.instance.WeaponLevelUp();
                        }
                        else
                        {
                            GameManager.instance.Upgrade(rouletteData.weapons[randNum].sprite, rouletteData.weapons[randNum]);
                            rouletteData.weapons.Remove(rouletteData.weapons[randNum]);
                        }

                        if (GameManager.instance.GetLevel(2) == 4)
                            insertCoinButton.interactable = false;
                    }
                    break;
            }

        }else if(answer == 1) // ½ÇÆÐ ->  ÇÏÆ® ¾ò±â
        {
            GameManager.instance.AddHP();
        }
        else // ½ÇÆÐ -> ²Î
        {
            print("²Î!");
        }

    }

    /// <summary>
    /// È®·ü ¸®½ºÆ®¸¦ °¡Áö°í ·ê·¿ °ª ¼³Á¤
    /// </summary>
    /// <param name="probs">È®·ü ¸®½ºÆ®</param>
    /// <returns></returns>
    private int Choose(List<float> probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Count; i++)
        {
            if (randomPoint < probs[i])
            {
                return answer = i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return answer = probs.Count - 1;
    }

    // coin ¹öÆ° Å¬¸¯ -> ·ê·¿ ½ÇÇà
    public void PlayRoulette()
    {
        if(GameManager.instance.GetCurrentCoin() >= rouletteData.coin)
        {
            GameManager.instance.UseCoin(rouletteData.coin);
        }
        else
        {
            return;
        }

        StartCoroutine(StartRoulette());
    }

    // ·ê·¿ Á¾·á
    public void ExitRoulette()
    {
        gameObject.SetActive(false);
        upgradeUI.SetActive(true);
    }
}
