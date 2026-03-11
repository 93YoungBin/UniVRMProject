using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class AvatarManager : MonoBehaviour , IPunObservable
{
    // 아바타 얼굴 변형을 위한 BlendShape
    private VRMBlendShapeProxy avatar;

    // 얼굴 정보 갱신을 위한 FaceData Class
    private FaceData faceData = new FaceData();

    // 아바타 변경을 위한 BlendShape 캐싱
    private VRMBlendShapeProxy[] avatars;

    // Face Class Data 동기화를 위한 string
    private string tempstring;

    //RPC 동기화를 위한 Photon View
    private PhotonView photonView;

    private void Awake()
    {
        avatars = GetComponentsInChildren<VRMBlendShapeProxy>();
        photonView = GetComponent<PhotonView>();
        SetAvatar(1);
    }

    public void SetAvatarRPC(int num)
    {
        photonView.RPC("SetAvatar", RpcTarget.All,num);
    }

    [PunRPC]
    public void SetAvatar(int num)
    {
        foreach (var item in avatars)
        {
            item.gameObject.SetActive(false);
        }

        avatar = avatars[num];
        avatars[num].gameObject.SetActive(true);
    }

    public void UpdateFaceData(float left,float right,float mouth)
    {
        faceData.LeftEye = left;
        faceData.RightEye = right;
        faceData.Mouth = mouth;
    }
    private void Update()
    {
        if(avatar == null)
        {
            return;
        }

        avatar.ImmediatelySetValue(
            BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_L),
            faceData.LeftEye);

        avatar.ImmediatelySetValue(
            BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_R),
            faceData.RightEye);

        avatar.ImmediatelySetValue(
            BlendShapeKey.CreateFromPreset(BlendShapePreset.A),
            faceData.Mouth);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(JsonUtility.ToJson(faceData));
        }
        else
        {
            tempstring = (string)stream.ReceiveNext();
            faceData = JsonUtility.FromJson<FaceData>(tempstring);
        }
    }
}

public class FaceData
{
    public float LeftEye;
    public float RightEye;
    public float Mouth;
}