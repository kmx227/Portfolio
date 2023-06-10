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

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static SceneLoadManager Instance
    {
        get
        {
            if (null == instance)
            {
                //게임 인스턴스가 없다면 하나 생성해서 넣어준다.
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