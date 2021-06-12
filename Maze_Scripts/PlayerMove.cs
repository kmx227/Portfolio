using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float turnSpeed = 4.0f; // 마우스 회전 속도    
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )
    public float moveSpeed = 4.0f; // 이동 속도
    int clickCount = 0;
    bool iDown;
    public bool[] iUse;
    bool onClick;
    bool mouse;
    public GameObject[] Item;
    public bool[] hasItem;
    public bool n3Click;

    public GameObject nearObject;
    public GameObject[] ItemImage;
    public GameObject paint;
    public GameObject miniMap;
    public GameObject guide;

    float distance = 0.5f;
    RaycastHit rayHit;
    Ray ray;

    FlashLight theLight;
    Change Ontext;
    Drawing Draw;
    ending end;
    [SerializeField] private AudioClip stepSound;
    private AudioSource p_AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray();
        Ontext = FindObjectOfType<Change>();
        Draw = FindObjectOfType<Drawing>();
        p_AudioSource = GetComponent<AudioSource>();
        end = FindObjectOfType<ending>();
    }

    // Update is called once per frame
    void Update()
    {
        p_AudioSource.clip = stepSound;
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        Vector3 move =
            transform.forward * v +
            transform.right * h;

        transform.position += move * moveSpeed * Time.deltaTime;
        if(v != 0 || h != 0)
        {
            PlayStepSound();
        }
        

        ray.origin = this.transform.position;
        ray.direction = this.transform.forward;

        if (Physics.Raycast(ray.origin, ray.direction, out rayHit, distance))
        {
            if (distance > 0.4f)
            {
                Debug.Log(rayHit.collider.gameObject.name);
                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    if (!n3Click)
                    {
                        Draw.DrawMouse();
                    }
                }
            }
        }

        input();
        Interaction();
        toItem();
        toClick();

        if (mouse)
        {
            guide.SetActive(false);
        }
    }

    private void PlayStepSound()
    {
        p_AudioSource.clip = stepSound;
        p_AudioSource.Play();
    }

    void input()
    {
        iDown = Input.GetButtonDown("Interaction");
        iUse[0] = Input.GetButtonDown("Item1");
        iUse[1] = Input.GetButtonDown("Item2");
        iUse[2] = Input.GetButtonDown("Item3");
        iUse[3] = Input.GetButtonDown("Item4");
        onClick = Input.GetButtonDown("LightOn");
        mouse = Input.GetMouseButtonDown(0);
    }

    void Interaction()
    {
        if(iDown && nearObject != null)
        {
            if(nearObject.tag == "Item")
            {
                Debug.Log("!");
                Item item = nearObject.GetComponent<Item>();
                int ItemIndex = item.value;
                hasItem[ItemIndex] = true;

                Destroy(nearObject);

                for (int i = 0; i < 5; i++)
                {
                    if (item.value == i)
                    {
                        item.Items[i] = item.realItem;
                        if (i < 4)
                        {
                            ItemImage[i].SetActive(true);
                        }
                        Debug.Log(i+1 + "번 아이템 획득");

                        if (item.realItem.Equals(item.Items[4]))
                        {
                            item.handItem.SetActive(true);
                            theLight = FindObjectOfType<FlashLight>();
                            theLight.isClick = true;
                        }
                    }
                }
            }
        }
    }

    void toItem()
    {

        for (int i = 0; i < 4; i++)
        {
            if (iUse[i] && hasItem[i])
            {
                Debug.Log(i + "!!!!!!!!!");

                Debug.Log(i + 1 + "번 아이템 사용");
             
                if (i != 2)
                {
                    ItemImage[i].SetActive(false);
                    hasItem[i] = false;
                }

                if (iUse[0])
                {
                    theLight.gameObject.SetActive(true);
                    theLight.isClick = true;
                }
                else if (iUse[1])
                {
                    Ontext.onText = true;
                    Ontext.goStageUI.GetComponent<Change>().onText = true;
                }
                else if (iUse[2] && n3Click)
                {
                    paint.GetComponent<Image>().color = Color.blue;
                    iUse[2] = !iUse[2];
                    n3Click = false;
                   
                }
                else if (iUse[2] && !n3Click)
                {
                    paint.GetComponent<Image>().color = Color.white;
                    iUse[2] = !iUse[2];
                    n3Click = true;
                }
                else if (iUse[3])
                {
                    miniMap.SetActive(true);
                    Invoke("mini", 30f);
                }
            }
        }
    }

    void mini()
    {
        miniMap.SetActive(false);
    }

    void toClick()
    {
        if (theLight)
        {
            if (onClick && theLight.isClick)
            {
                if(clickCount == 0)
                {//theLight = FindObjectOfType<FlashLight>();
                    Debug.Log("onClick 버튼 눌림");
                    theLight.gameObject.SetActive(false);
                    clickCount = 1;
                    Debug.Log(clickCount);
                }
                else if(clickCount == 1)
                {
                    theLight.gameObject.SetActive(true);
                    clickCount = 0;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
        {
            nearObject = other.gameObject;
            Debug.Log(nearObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            nearObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Clear")
        {
            end.Clear();
        }
        else if(other.tag == "Monster")
        {
            end.End();
        }
    }
}
