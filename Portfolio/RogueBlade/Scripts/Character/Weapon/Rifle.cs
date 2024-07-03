using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] GameObject character;
    [SerializeField] WeaponData weaponData;
    [SerializeField] GameObject bulletEffect;

    private float power;
    private int count;
    private float range;
    private bool onShot = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        power = weaponData.power;
        count = weaponData.count;
        range = weaponData.range;
    }

    // Update is called once per frame
    void Update()
    {
        if (onShot) return;

        var targets = Physics2D.OverlapCircleAll(character.transform.position, range, LayerMask.GetMask("Block", "Bomb"));
        if (targets.Length > 0)
        {
            onShot = true;
            if (bulletEffect.activeSelf == false) bulletEffect.SetActive(true);
            GameObject nearestTarget = null;
            foreach(var target in targets)
            {
                var maxDist = 100f;
                var dist = Vector2.Distance(character.transform.position, target.transform.position);
                if (dist < maxDist)
                {
                    maxDist = dist;
                    nearestTarget = target.gameObject;
                }
            }

            var bullet = ObjectPool.GetObject(3);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().Init(nearestTarget, power);
            StartCoroutine(ResetBullet());
        }
        else
        {
            bulletEffect.SetActive(false);
        }
    }

    IEnumerator ResetBullet()
    {
        yield return new WaitForSeconds(0.2f);
        onShot = false;
    }
}
