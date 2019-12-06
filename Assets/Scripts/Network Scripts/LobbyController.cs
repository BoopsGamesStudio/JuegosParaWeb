using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject StartButton;
    [SerializeField]
    private GameObject CancelButton;
    [SerializeField]
    private int roomSize;
    [SerializeField]
    private GameObject LoadingButton;
    Canvas mainCanvas;

    private void Awake()
    {
        foreach (Canvas canv in FindObjectsOfType<Canvas>())
        {
            if (canv.name == "AltCanvas")
                continue;

            mainCanvas = canv;
        }

        if (Application.isMobilePlatform)
            mainCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Lobby");
        PhotonNetwork.AutomaticallySyncScene = true;
        StartButton.SetActive(true);
        LoadingButton.SetActive(false);
    }

    public void StartBtn()
    {
        StartButton.SetActive(false);
        CancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();

    }

    public void BackBtn()
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(0);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Creating Room");
        int rndRoomNumber = Random.Range(0, 10);
        RoomOptions options = new RoomOptions()
        { IsOpen = true, IsVisible = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + rndRoomNumber, options);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Fallo al crear la sala, intentando de nuevo");
        CreateRoom();
    }

    public void CancelBtn()
    {
        CancelButton.SetActive(false);
        StartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
