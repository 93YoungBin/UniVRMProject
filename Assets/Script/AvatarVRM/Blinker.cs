using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class Blinker : MonoBehaviour , IPunObservable
{
    public VRMBlendShapeProxy proxy;

    private float lefteye;
    private float righteye;
    private float jar;

    void Start()
    {
        proxy = GetComponent<VRMBlendShapeProxy>();
    }
    public void SetBlink(float left, float right)
    {
        lefteye = left;
        righteye = right;
    }

    public void SetMouth(float open)
    {
        jar = open;
    }

    private void Update()
    {
        proxy.ImmediatelySetValue(
            BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_L),
            lefteye);

        proxy.ImmediatelySetValue(
            BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_R),
            righteye);

        proxy.ImmediatelySetValue(
            BlendShapeKey.CreateFromPreset(BlendShapePreset.A),
            jar);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(jar);
        }
        else
        {
            jar = (float)stream.ReceiveNext();
        }
    }
}
