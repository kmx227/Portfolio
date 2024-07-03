using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public static class Extensions
{
    private static System.Random rand = new System.Random();

    public static void Shuffle<T>(this IList<T> values)
    {
        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
    }
}

public class PhaseManager : MonoBehaviour
{
    public enum Phase { Phase1 = 0 , Phase2 = 1, Phase3 = 2, Phase4 = 3, PhaseBoss = 4 }

    public Phase currentPhase;
    private bool OnPhase = false;
    private bool canSpawn = true;
    private float totalPhaseProgress = 0f;

    private Queue<int> phaseQueue = new Queue<int>();

    [SerializeField] PhaseSecond phaseSecond;
    [SerializeField] PhaseThird phaseThird;

    private delegate void PhaseDelegate(int phase);
    PhaseDelegate phaseDelegate;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public int GetPhase()
    {
        return (int)currentPhase;
    }

    public float GetProgress()
    {
        return totalPhaseProgress;
    }

    public void CheckOnPhase(bool onPhase)
    {
        this.OnPhase = onPhase;
    }

    private void ChangePhase()
    {
        currentPhase += 1;
        Init();
    }

    private void Init()
    {
        if((int)currentPhase > 0)
        {
            if((int)currentPhase == 4)
            {
                for (int i = 0; i < 10; i++)
                {
                    phaseQueue.Enqueue(1);
                }
            }
            else
            {
                var phaseCount = UnityEngine.Random.Range(2, 4);
                var blockCount = 10 - phaseCount;
                var list = new List<int>();

                for (int i = 0; i < phaseCount; i++)
                {
                    list.Add(1);
                }
                for (int i = 0; i < blockCount; i++)
                {
                    list.Add(0);
                }

                list.Shuffle();
                foreach (int i in list)
                {
                    phaseQueue.Enqueue(i);
                }
            }

            switch (currentPhase)
            {
                case Phase.Phase2:
                    phaseDelegate += phaseSecond.PhaseStart;
                    break;
                case Phase.Phase3:
                    phaseDelegate -= phaseSecond.PhaseStart;
                    phaseDelegate += GameManager.instance.SpawnPhaseBlock;
                    break;
                case Phase.Phase4:
                    phaseDelegate -= GameManager.instance.SpawnPhaseBlock;
                    phaseDelegate += phaseThird.StartPhase;
                    break;
                case Phase.PhaseBoss:
                    phaseDelegate += phaseSecond.PhaseStart;
                    phaseDelegate += GameManager.instance.SpawnPhaseBlock;
                    phaseDelegate += phaseThird.StartPhase;
                    break;

            }
        }
        else if((int)currentPhase == 0)
        {
            for(int i = 0; i < 10; i++)
            {
                phaseQueue.Enqueue(0);
            }
        }

        if((int)currentPhase == 4)
        {
            var randomNum = UnityEngine.Random.Range(1, 4);
            StartCoroutine(StartPhase(randomNum, true));
        }
        else
        {
            StartCoroutine(StartPhase((int)currentPhase, false));
        }  
    }

    IEnumerator StartPhase(int phase, bool random)
    {
        totalPhaseProgress += 2.5f;
        yield return new WaitForSeconds(3);
        var num = phaseQueue.Dequeue();

        var phaseNum = phase;
        if (random == true)
        {
            phaseNum = UnityEngine.Random.Range(1, 4);
        }

        switch (num)
        {
            case 0:
                GameManager.instance.SpawnBlocks();
                canSpawn = false;
                yield return null;
                break;
            case 1:
                phaseDelegate(phaseNum);
                OnPhase = true;
                yield return null;
                break;
        }
        yield return new WaitUntil(() => GameManager.instance.GetCurrentBlockGroupCount() <= 0);
        canSpawn = true;
        yield return new WaitUntil(() => OnPhase == false);
        
        if(OnPhase == false && canSpawn == true)
        {
            if (phaseQueue.Count > 0)
                if(random == true)
                {
                    StartCoroutine(StartPhase(4, true));
                }
                else
                {
                    StartCoroutine(StartPhase(phase, false));
                }
            else
                ChangePhase();
        }
    }
}
