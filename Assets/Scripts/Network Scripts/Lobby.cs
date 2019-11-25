using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    public Button PlayButton;
    public Text Log;

    public byte maxPlayersInRoom = 4;
    public byte minPlayersInRoom = 2;

    public int playerCount;
    public Text playerCountTxt;

    public void Connect()
    {
        if(!PhotonNetwork.IsConnected)
        {
            if(PhotonNetwork.ConnectUsingSettings())
            {
                PlayButton.interactable = false;
                Log.text += "\nConnecting...";
            } else
            {
                Log.text += "\nError on connection";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Log.text += "\nConnected succesfully to region " + PhotonNetwork.CloudRegion;
        JoinRandom();
    }

    public void JoinRandom()
    {
        if(!PhotonNetwork.JoinRandomRoom())
        {
            Log.text += "\nFailed to join room";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log.text += "\nRoom to join not found, creating new one...";

        if(PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersInRoom }))
        {
            Log.text += "\nRoom created succesfully";
        } else
        {
            Log.text += "\nFailed to create room";
        }
    }

    public override void OnJoinedRoom()
    {
        Log.text += "\nJoined room";
    }

    void FixedUpdate()
    {
        if(PhotonNetwork.CurrentRoom != null)
        {
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }

        playerCountTxt.text = playerCount + "/" + maxPlayersInRoom;
    }
}
