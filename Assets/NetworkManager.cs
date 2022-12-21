using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //instance
    public static NetworkManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // connecting to master server
        PhotonNetwork.ConnectUsingSettings();
    }


    // Creates or joins a new room
    public void CreateOrJoinRoom()
    {
        // Join if there is a room to join
        if (PhotonNetwork.CountOfRooms > 0)
            PhotonNetwork.JoinRandomRoom();

        // Creates a new room if there is no room to join
        else
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(null, options);
        }
    }

    // Used in scene transition
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
