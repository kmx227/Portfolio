using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class playerMove : MonoBehaviourPun, IPunObservable
{
    public float MaxSpeed;
    public float JumpPower;
    int jumpCount = 2;
    public Animator anim;
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Image NickNameText;
    Vector3 curPos;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        NickNameText.color = PV.IsMine ? Color.green : Color.red;
    }

    void Update()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        Debug.Log(PV + "");
        if (PV.IsMine)
        {
            if (RB.velocity.x > 0)
            {
                Debug.DrawRay(RB.position, Vector3.right, new Color(0, 1, 0));

                RaycastHit2D rayHit = Physics2D.Raycast(RB.position, Vector3.right, 1, LayerMask.GetMask("JumpWall"));

                if (rayHit.collider != null)
                {
                    if (rayHit.distance < 0.3f)
                        anim.SetBool("isJumping", false);
                    jumpCount = 1;
                }



            }
            if (RB.velocity.x < 0)
            {
                Debug.DrawRay(RB.position, Vector3.left, new Color(0, 1, 0));

                RaycastHit2D rayHit = Physics2D.Raycast(RB.position, Vector3.left, 1, LayerMask.GetMask("JumpWall"));

                if (rayHit.collider != null)
                {
                    if (rayHit.distance < 0.3f)
                        anim.SetBool("isJumping", false);
                    jumpCount = 1;
                }



            }
            //Jump

            if (Input.GetButtonDown("Jump") && jumpCount > 0)
            {
                Debug.Log(jumpCount);
                jumpCount--;
                if (jumpCount > 2) return;
                RB.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);

            }

            //Stop Speed
            if (Input.GetButtonUp("Horizontal"))
            {

                RB.velocity = new Vector2(RB.velocity.normalized.x * 0.5f, RB.velocity.y);
            }

            //Direction Sprite
            if (axis != 0)
            {
                //SR.flipX = axis == -1;
                PV.RPC("FlipX", RpcTarget.All, axis);
            }

            //Animation
            if (Mathf.Abs(RB.velocity.x) < 0.6)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
            }
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100)
        {
            transform.position = curPos;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
        }

        if (Input.GetButtonDown("test"))
        {
            print("1¹ø");

        }
    }

    [PunRPC]
    void FlipX(float axis) => SR.flipX = axis == -1;

    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            //Move Speed
            float h = Input.GetAxisRaw("Horizontal");
            RB.AddForce(Vector2.right * h * 2, ForceMode2D.Impulse);


            if (RB.velocity.x > MaxSpeed) //Right MaxSpeed
                RB.velocity = new Vector2(MaxSpeed, RB.velocity.y);
            else if (RB.velocity.x < MaxSpeed * (-1)) // Left MaxSpeed
                RB.velocity = new Vector2(MaxSpeed * (-1), RB.velocity.y);

            //Landing Platform
            if (RB.velocity.y < 0)
            {
                Debug.DrawRay(RB.position, Vector3.down, new Color(0, 1, 0));

                RaycastHit2D rayHit = Physics2D.Raycast(RB.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

                if (rayHit.collider != null)
                {
                    if (rayHit.distance < 0.5f)
                        anim.SetBool("isJumping", false);
                    jumpCount = 2;
                }
            }
        }
       
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
}
