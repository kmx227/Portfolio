using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;

    public float h;
    public float v;
    Vector3 moveVec;
    Vector3 curPos;

    public void Awake()
    {
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.red;
    }

    public void Update()
    {
        if (PV.IsMine)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            moveVec = new Vector3(h, v, 0).normalized;
            transform.position += moveVec * 1 * Time.deltaTime;
        }
        else if((transform.position - curPos).sqrMagnitude >= 100)
        {
            transform.position = curPos;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
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
