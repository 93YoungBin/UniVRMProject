using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
        private void Start()
        {
            // Photon 서버에 연결
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // 새로운 룸 생성
            PhotonNetwork.CreateRoom("TestRoom", new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
        }

        public override void OnJoinedRoom()
        {
            // 룸에 입장했을 때 플레이서 수 체크
            Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");
    }
}
