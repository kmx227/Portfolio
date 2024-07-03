using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlockManager : MonoBehaviour
{
    private Queue<List<Block>> blockGroups = new Queue<List<Block>>(); //블럭 그룹을 가진 리스트
    private static List<Block> currentBlockGroup = new List<Block>(); // 현재 가까운 블럭그룹

    // Update is called once per frame
    void Update()
    {
        if (currentBlockGroup.Count <= 0 && blockGroups.Count > 0) DequeueBlockGroup();

        if (Input.GetKeyDown(KeyCode.Alpha1)) SpawnBlocks(false);
    }

    private List<Block> CreateBlocks(bool phaseThird)
    {
        var blocks = new List<Block>();
        var randNum = Random.Range(5, 11);
        var list = new List<int>();

        // 블록이 7개 이상이고, 3번째 페이즈 일때 특수 블록 생성
        // 생성 방식, 리스트 안에서 0->기본블럭 / 1->특수 블럭
        if (randNum > 6 && phaseThird == true)
        {
            var phaseCount = UnityEngine.Random.Range(1, 3);
            var blockCount = randNum - phaseCount;

            for (int j = 0; j < phaseCount; j++)
            {
                list.Add(1);
            }
            for (int j = 0; j < blockCount; j++)
            {
                list.Add(0);
            }

            list.Shuffle();
        }

        for (int i=0; i<randNum; i++)
        {
            Block block;
            if(list.Count > 0)
            {
                if (list[i] == 0)
                {
                    block = GetBlock(0, i);
                }
                else
                {
                    block = GetBlock(1, i);
                }
            }
            else
            {
                block = GetBlock(0, i);
            }

            blocks.Add(block);
        }

        return blocks;
    }

    private Block GetBlock(int num, int index)
    {
        var block = ObjectPool.GetObject(num).GetComponent<Block>();
        block.InitType(num);
        block.transform.transform.position = new Vector2(transform.position.x, block.transform.position.y + index + 1);
        return block;
    }

    public void EnqueueBlockGroup(List<Block> blocks)
    {
        blockGroups.Enqueue(blocks);
    }

    public void DequeueBlockGroup()
    {
        if (blockGroups.Count <= 0) return;
        
        currentBlockGroup = blockGroups.Dequeue();
    }

    public static int CurrentBlockCount()
    {
        return currentBlockGroup.Count;
    }

    // currentBlockGroup안에서 해당 블록 제거
    public static void RemoveBlockInCurrentGroup(Block block)
    {
        currentBlockGroup.Remove(block);

        if (currentBlockGroup.Count <= 0) GameManager.instance.CheckParamToNextOfPhase();
    }

    // currentBlockGroup을 전체 제거
    public void RemoveCurrentGroup()
    {
        for (int i = currentBlockGroup.Count - 1; i >= 0; i--)
        {
            currentBlockGroup[i].DestroyBlock(false);

        }

        GameManager.instance.CheckParamToNextOfPhase();
    }

    public void SpawnBlocks(bool phaseThird)
    {

        EnqueueBlockGroup(CreateBlocks(phaseThird));
    }

    public static List<Block> GetCurrentBlockList()
    {
        return currentBlockGroup;
    }
}
