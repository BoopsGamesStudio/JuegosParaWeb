using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    private PhotonView PV;

    [SerializeField]
    private int multiplayerSceneIndex;
    [SerializeField]
    private int lobbySceneIndex;

    private int playerCount;
    private int roomSize;
    [SerializeField]
    private int minPlayersToStart;

    [SerializeField]
    private Text playerCountDisplay;
    [SerializeField]
    private Text timerToStartDisplay;

    private bool readyToStart;
    private bool startingGame;

    private float timerToStartGame;

    [SerializeField]
    private float maxWaitTime;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        timerToStartGame = maxWaitTime;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;

        PlayerCountUpdate();
    }

    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        playerCountDisplay.text = playerCount + "/" + roomSize;

        if (playerCount == roomSize)
            readyToStart = true;
        else
            readyToStart = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();

        if (PhotonNetwork.IsMasterClient)
            PV.RPC("RPC_SenTimer", RpcTarget.Others, timerToStartGame);
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    private void Update()
    {
        WaitingForMorePlayers();
    }

    private void WaitingForMorePlayers()
    {
        if (playerCount < minPlayersToStart)
            ResetTimer();

        if (readyToStart)
            timerToStartGame -= Time.deltaTime;

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if(timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
        }

        
    }

    private void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
    }

    public void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void CancelBtn()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(lobbySceneIndex);
    }
}
