using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1"; //game version
    public string userId = "Daye";
    public byte maxPlayer = 2;

    public Text connectionInfoText; //네트워크 정보를 표시할 텍스트
    public Button joinButton; //룸 접속 버튼

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion; //접속에 필요한 정보(게임정보)설정
        PhotonNetwork.NickName = userId;

        PhotonNetwork.ConnectUsingSettings(); //설정한 정보를 가지고 마스터 서버 접속시도
        joinButton.interactable = false; //룸 접속 버튼을 잠시 비활성화
        connectionInfoText.text = "마스터 서버에 접속중..";
    }

    public override void OnConnectedToMaster() //마스터 서버 접속 시 자동실행
    {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }
    public override void OnDisconnected(DisconnectCause cause) //마스터 서버 접속 실패 시 자동실행
    {
        joinButton.interactable = false; //룸 접속 버튼 비활성화
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음 접속 재 시도중...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect() //룸 접속 시도 (마스터 서버 연결 후)
    {
        joinButton.interactable = false; //중복접속 시도를 막기위해 접속 버튼 잠시 비활성화
        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "룸에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }
        else //마스터 서버에 접속중이 아니라면, 마스터 서버에 접속시도
        {
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음 접속 재 시도중...";
            PhotonNetwork.ConnectUsingSettings(); //마스터 서버로의 재접속 시도
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    //(빈 방이 없어) 랜덤 룸 참가에 실패한 경우 자동실행
    {
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성..";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom() //룸에 참가 완료된 경우 자동실행
    {
        connectionInfoText.text = "방 참가 성공";
        PhotonNetwork.LoadLevel("Game"); //모든 룸 참가자들이 Main Scene(FPS)를 load하게 함
        //PhotonNetwork.Instantiate("Player", new Vector3(38, 11, 26), Quaternion.identity);
    }
}