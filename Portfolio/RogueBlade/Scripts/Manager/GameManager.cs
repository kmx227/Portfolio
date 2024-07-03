using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager() { }

    private int Exp = 0;
    private int gameHP = 3;
    private int coin = 0;
    private static int[] Level = new int[3] { 1, 1, 1 };

    [SerializeField] private Shield shield;
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject rouletteUI;
    [SerializeField] private GameObject buttonUI;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private PhaseManager phaseManager;

    static public GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public int GetCurrentCoin()
    {
        return coin;
    }

    public int GetCurrentExp()
    {
        return Exp;
    }

    public int GetGameHP()
    {
        return gameHP;
    }

    public int GetLevel(int num)
    {
        return Level[num]; 
    }

    public float GetCurrentPhaseProgress()
    {
        return phaseManager.GetProgress();
    }
    
    public int GetCurrentPhase()
    {
        return phaseManager.GetPhase();
    }

    public int GetCurrentShieldLevel()
    {
        return shield.GetLevel();
    }

    public int GetCurrentAttackLevel()
    {
        return weapon.GetLevel();

    }

    public int GetCurrentBlockGroupCount()
    {
        return BlockManager.CurrentBlockCount();
    }

    public void UseCoin(int pay)
    {
        coin -= pay;
    }

    public void WeaponLevelUp()
    {
        Level[2]++;
    }

    public void AddHP()
    {
        if (gameHP < 3)
        {
            gameHP += 1;
        }
    }

    public void AddExp(int exp)
    {
        Exp += exp;
        coin += exp;

        if (Exp >= 100 )
        {
            Exp -= 100;
            StartCoroutine(OpenRouletteUI());
        }
    }

    public void Upgrade(int num, float power)
    {
        Level[num]++;
        if (num == 0)
        {
            shield.Upgrade(power);
        }
        else if (num == 1)
        { 
            weapon.Upgrade(power);
        }
    }

    public void Upgrade(Sprite sprite, WeaponData data)
    {
        Level[2]++;
        weapon.AddWeapon(sprite, data);
        
    }

    public void SpawnBlocks()
    {
        blockManager.SpawnBlocks(false);
    }

    public void SpawnPhaseBlock(int phase)
    {
        if (phase != 2) return;
        
        blockManager.SpawnBlocks(true);
    }

    public void SpawnRiflePet()
    {
        weapon.SpawnPet();
    }

    public void GameHPDamaged(bool onBlock)
    {
        gameHP -= 1;
        if (onBlock)
            blockManager.RemoveCurrentGroup();

        if (gameHP <= 0)
        {
            print("End");
        }
    }

    public void CheckParamToNextOfPhase()
    {
        phaseManager.CheckOnPhase(false);
    }

    IEnumerator OpenRouletteUI()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0;
        buttonUI.SetActive(false);
        rouletteUI.SetActive(true);
    }
}
