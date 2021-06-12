using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //출발지에 도착하면 박스는 못움직여야함, 그에 대한 변수
    public static bool s_canPresskey = true;

    //box이동 속도
    [SerializeField] float moveSpeed = 3;
    Vector3 dir = new Vector3(); //방향값
    public Vector3 destPos = new Vector3();//목적지
    //Vector3 orginPos = new Vector3();

    //box회전 구현
    [SerializeField] float spinSpeed = 270; //스피드
    Vector3 rotDir = new Vector3(); //방향
    Quaternion destRot = new Quaternion(); //목표 회전값

    //box의 들썩임 구현
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;

    //노트 연속판정 제한
    bool canMove = true;
    bool isFalling = false;

    [SerializeField] Transform fakeCube = null; //회전값을 구하기 위한 fakeCube
    [SerializeField] Transform realCube = null; //회전 시킬 객체


    //TimingManager참조1
    TimingManager theTimingManager;
    //카메라 참조1
    CameraController theCam;
    StageManager theStage;

    Rigidbody myRigid;

    private void Start()
    {
        //TimingManager참조2
        theTimingManager = FindObjectOfType<TimingManager>();
        //카메라 참조2
        theCam = FindObjectOfType<CameraController>();
        myRigid = GetComponentInChildren<Rigidbody>();
        theStage = FindObjectOfType<StageManager>();
        //orginPos = transform.position;
    }

    public void Initiallized()
    {
        transform.position = Vector3.zero;
        destPos = Vector3.zero;
        realCube.localPosition = Vector3.zero;
        canMove = true;
        s_canPresskey = true;
        isFalling = false;
        myRigid.useGravity = false;
        myRigid.isKinematic = true;
    }
    void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            //방향키를 눌렀을때
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                if (canMove && s_canPresskey && !isFalling)
                {
                    Calc();

                    // 판정 체크
                    if (theTimingManager.CheckTiming())
                    {
                        //이동구현
                        StartAction();
                    }
                }
            }
        }
    }

        void Calc()
        {
            //방향 계산(축을 기준으로 계산), getAxisRaw -1,0,1의 값 가짐
            dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

            //목적지 목표값 계산, 키 눌림 중복 방지
            if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
            {
                destPos = transform.position + new Vector3(-dir.x, 0, dir.z); //이동 목표값 계산
                rotDir = new Vector3(-dir.z, 0f, -dir.x); //회전 목표값 계산
            }

            //fakeCube를 회전 후 결과값 저장
            fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
            destRot = fakeCube.rotation;
        }

        void StartAction() {


            //이동 코루틴
            StartCoroutine(MoveCo());
            //회전 코루틴
            StartCoroutine(SpinCo());
            //들썩임 코루틴
            StartCoroutine(RecoilCo());
            //줌캠 코루틴
            StartCoroutine(theCam.ZoomCam());
        }

        //코루틴 생성
        IEnumerator MoveCo()
        {
            //제한
            canMove = false;

            //제곱이 0.001에 가까워질때까지 이동
            while (Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)
            {
                //프레임당
                transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            //근소한 차이 방지 위치 설정
            transform.position = destPos;

            //제한 해제
            canMove = true;
        }

        IEnumerator SpinCo()
        {
            //realCube와 목표회전값이 근소해질때까지 
            while (Quaternion.Angle(realCube.rotation, destRot) > 0.5f)
            {
                //realCube 회전
                realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
                yield return null;
            }

            realCube.rotation = destRot;
        }

        IEnumerator RecoilCo()
        {
            //최고 높이 까지의 반동
            while (realCube.position.y < recoilPosY)
            {
                realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
                yield return null;
            }

            //최고 높이 후 원래 위치로
            while (realCube.position.y > 0)
            {
                realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
                yield return null;
            }

            realCube.localPosition = new Vector3(0, 0, 0);
        }

        void CheckFalling()
        {
            if (!isFalling && canMove)
            {
                if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
                {
                    Falling();
                }
            }
        }

        void Falling()
        {
            isFalling = true;
            myRigid.useGravity = true;
            myRigid.isKinematic = false;
        }

        /*public void ResetFalling()
         {
             isFalling = false;
             myRigid.useGravity = false;
             myRigid.isKinematic = true;
             transform.position = orginPos;
             realCube.localPosition = new Vector3(0, 0, 0);
         }*/
    }
