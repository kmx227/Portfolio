using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Shield : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private int shieldLevel = 1;
    private float power;
    private bool isBlockable = true;
    private int count = 1;

    // Start is called before the first frame update
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        power = 8f;
    }

    public int GetLevel() { return shieldLevel; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            OpenShield();
        }

        if (isBlockable == false)
        {
            if(count == 1)
            {
                Blocking();
                count -= 1;
            }
        }
    }

    // 쉴드 활성화 시 파티클 생성
    public void OpenShield()
    {
        if (isBlockable == false) { return; }

        isBlockable = false;
        _particleSystem.Play();
    }

    private void Blocking()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1.5f, LayerMask.GetMask("Block"));

        if (hit.collider != null)
        {
            hit.collider.GetComponent<Block>().Lift(power);
        }

        StartCoroutine(TimerForShield());
    }

    // 쉴드 쿨타임
    IEnumerator TimerForShield()
    {
        yield return new WaitForSeconds(0.5f);
        isBlockable = true;
        count = 1;
    }


    public void Upgrade(float upgradePower)
    {
        shieldLevel++;
        power = 8f * upgradePower;
    }

    // 쉴드 상태 체크
    public bool OnShield()
    {
        return isBlockable == false;
    }
}
