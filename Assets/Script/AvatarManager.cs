using Photon.Pun;
using UnityEngine;
using VRM;

public class AvatarManager : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;

    // 현재 활성 아바타
    private VRMBlendShapeProxy avatar;

    // 모든 아바타
    private VRMBlendShapeProxy[] avatars;

    // 얼굴 데이터
    private FaceData faceData;

    // BlendShapeKey 캐싱
    private BlendShapeKey blinkL;
    private BlendShapeKey blinkR;
    private BlendShapeKey mouthA;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        avatars = GetComponentsInChildren<VRMBlendShapeProxy>(true);

        // BlendShapeKey 캐싱
        blinkL = BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_L);
        blinkR = BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_R);
        mouthA = BlendShapeKey.CreateFromPreset(BlendShapePreset.A);

        SetAvatar(0);
    }

    // Avatar 변경
    public void SetAvatarRPC(int index)
    {
        photonView.RPC(nameof(SetAvatar), RpcTarget.AllBuffered, index);
    }

    [PunRPC]
    public void SetAvatar(int index)
    {
        if (index < 0 || index >= avatars.Length)
            return;

        foreach (var a in avatars)
        {
            a.gameObject.SetActive(false);
        }

        avatar = avatars[index];
        avatar.gameObject.SetActive(true);
    }


    // 얼굴 데이터 갱신 (Master)
    public void UpdateFaceData(float left, float right, float mouth)
    {
        if (!photonView.IsMine)
            return;

        faceData.LeftEye = left;
        faceData.RightEye = right;
        faceData.Mouth = mouth;
    }


    private void Update()
    {
        if (avatar == null)
            return;

        avatar.ImmediatelySetValue(blinkL, faceData.LeftEye);
        avatar.ImmediatelySetValue(blinkR, faceData.RightEye);
        avatar.ImmediatelySetValue(mouthA, faceData.Mouth);
    }


    // Photon 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(faceData.LeftEye);
            stream.SendNext(faceData.RightEye);
            stream.SendNext(faceData.Mouth);
        }
        else
        {
            faceData.LeftEye = (float)stream.ReceiveNext();
            faceData.RightEye = (float)stream.ReceiveNext();
            faceData.Mouth = (float)stream.ReceiveNext();
        }
    }
}

// 얼굴 데이터 구조체
[System.Serializable]
public struct FaceData
{
    public float LeftEye;
    public float RightEye;
    public float Mouth;
}