using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{
    // 버튼 선택을 위한 캔버스
    private GameObject canvas;

    // 시점에 따른 분기처리를 위한 오브젝트 캐싱
    private GameObject[] viewer = new GameObject[2];

    // 권한 획득을 위한 포톤 뷰
    private PhotonView photonView;

    private void Awake()
    {
        canvas = transform.GetChild(0).gameObject;
        viewer[0] = transform.GetChild(1).gameObject;
        viewer[1] = transform.GetChild(2).gameObject;
        photonView = transform.GetChild(3).GetComponent<PhotonView>();
    }

    // 분기처리 적용
    public void SetType(int type)
    {
        canvas.SetActive(false);

        switch (type)
        {
            case 0:
            default:
                viewer[0].SetActive(true);
                Debug.Log("RequestOwnership");
                photonView.RequestOwnership();
                break;

            case 1:
                viewer[1].SetActive(true);
                break;
        }
    }

    public void RequestOwner()
    {
        photonView.RequestOwnership();
    }
}
