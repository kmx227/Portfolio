using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] StageManager _stageManager;

    public static int currentStage = 0;


    private static SceneLoadManager instance;

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
    public static SceneLoadManager Instance
    {
        get
        {
            if (null == instance)
            {
                //���� �ν��Ͻ��� ���ٸ� �ϳ� �����ؼ� �־��ش�.
                instance = new SceneLoadManager();
            }
            return instance;
        }
    }

    public SceneLoadManager()
    {

    }

    public static void StageUp()
    {
        currentStage += 1;
    }

    public static void OpenStage()
    {
        SceneManager.LoadScene(0);
    }

    public static void NextStage()
    {
        StageUp();
        SceneManager.LoadScene(0);
    }

    public static void OpenBoss()
    {
        SceneManager.LoadScene(1);
    }

#if UNITY_EDITOR



#endif
}