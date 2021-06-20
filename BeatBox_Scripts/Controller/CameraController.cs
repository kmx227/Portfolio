using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //카메라 타겟
    [SerializeField] Transform thePlayer = null;
    //스피드 변수
    [SerializeField] float followSpeed = 15;

    //플레이어 거리차이
    Vector3 playerDistance = new Vector3();

    float hitDistance = 0;
    //시야확보
    [SerializeField] float zoomDistance = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        //카메라위치 - 플레이어위치
        playerDistance = transform.position - thePlayer.position;
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어 따라다니기
        Vector3 t_destPos = thePlayer.position + playerDistance + transform.forward * hitDistance;
        //자연스러운 움직임 부여
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed * Time.deltaTime);
    }

    //코루치 생성
    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        //원상복구
        yield return new WaitForSeconds(0.15f);

        hitDistance = 0;
    }
}
