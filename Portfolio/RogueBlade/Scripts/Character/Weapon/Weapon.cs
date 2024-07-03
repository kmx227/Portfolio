using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int hasWeaponType;
    [SerializeField] private Sprite[] weaponSprites;
    [SerializeField] private List<Sprite> hasWeapons;
    [SerializeField] private List<WeaponData> weaponData;
    [SerializeField] private GameObject pet;
    [SerializeField] private Button changBtn;
    private bool isAttackable = true;

    [SerializeField]private int currentMaxWeaponCount = 1;
    [SerializeField] private int weaponlevel = 1;
    private float levelDamage;
    [SerializeField] private float attackDamage = 0;
    private int attackCount = 0;
    private float attackRange = 0;

    public float AttackRange { get { return attackRange; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();  
    }

    private void Start()
    {
        levelDamage = 1;
        attackDamage = weaponData[hasWeaponType].power;
        attackCount = weaponData[hasWeaponType].count;
        attackRange = weaponData[hasWeaponType].range;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (isAttackable == false) return;

            isAttackable = false;
            animator.enabled = true;
            animator.Rebind();
            animator.SetInteger("CurrentWeaponType", weaponData[hasWeaponType].ID);
            animator.SetTrigger("OnAttack");
            StartCoroutine(TimerForAttack());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            animator.enabled = false;
            hasWeaponType++;
            if (hasWeaponType >= currentMaxWeaponCount)
            {
                hasWeaponType = 0;
            }

            ChangeWeapon(hasWeaponType);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            GameManager.instance.Upgrade(1, 5f);
        }

        if (currentMaxWeaponCount > 1) changBtn.interactable = true;
    }

    public void OnAttack()
    {
        if (isAttackable == false) return;

        isAttackable = false;
        animator.enabled = true;
        animator.Rebind();
        animator.SetInteger("CurrentWeaponType", weaponData[hasWeaponType].ID);
        animator.SetTrigger("OnAttack");
        StartCoroutine(TimerForAttack());
    }

    public void ChangeAction()
    {
        animator.enabled = false;
        hasWeaponType++;
        if (hasWeaponType >= currentMaxWeaponCount)
        {
            hasWeaponType = 0;
        }

        ChangeWeapon(hasWeaponType);
    }

    public int GetLevel() { return weaponlevel; }

    private void ChangeWeapon(int count)
    {
        spriteRenderer.sprite = hasWeapons[count];
        gameObject.GetComponentInParent<Character>().ChangeWeapon(count);
        attackDamage = weaponData[count].power * levelDamage;
        attackCount = weaponData[count].count;
        attackRange = weaponData[count].range;
    }

    public int GetWeaponType() { return hasWeaponType; }

    public void AddWeapon(Sprite sprite, WeaponData data)
    {
        currentMaxWeaponCount++;

        hasWeapons.Add(sprite);
        weaponData.Add(data);
    }

    public void OnAttack(GameObject[] blocks)
    {
        if(blocks.Length < attackCount) attackCount = blocks.Length;

        if (weaponData[hasWeaponType].isCollison)
        {
            blocks[0].GetComponent<Block>().Lift(8f);
        }

        for (int i = 0; i < attackCount; i++)
        {
            blocks[i].GetComponent<Block>().OnDamaged(attackDamage);
        }
    }

    IEnumerator TimerForAttack()
    {
        yield return new WaitForSeconds(0.2f);
        isAttackable = true;
    }

    public void Upgrade(float power)
    {
        weaponlevel++;
        levelDamage = power;
        attackDamage = weaponData[hasWeaponType].power * levelDamage;
    }

    public void SpawnPet()
    {
        pet.SetActive(true);
    }
}
