using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;

    Vector3 moveVec;
    private GameObject nearObject;
    public Camera cam; //메인카메라

    public float sensitivity = 500f; //감도 설정
    float rotationX = 0.0f;  //x축 회전값
    float rotationY = 0.0f;  //z축 회전값

    private void Start()
    {

    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        transform.position += moveVec * speed * Time.deltaTime;
        //transform.LookAt(transform.position + moveVec);

        MouseSencer();
        moveTo();
    }

    void moveTo()
    {
        Debug.DrawRay(transform.position, Vector3.forward * 10, Color.blue);
    }

    public void MouseSencer()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        rotationX += x * sensitivity * Time.deltaTime;
        rotationY += y * sensitivity * Time.deltaTime;

        if (rotationY > 30)
        {
            rotationY = 30;
        }
        else if (rotationY < -30)
        {
            rotationY = -30;
        }
        transform.eulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
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
}
