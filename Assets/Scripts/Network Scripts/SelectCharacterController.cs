using Lean.Localization;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterController : MonoBehaviour
{
    [SerializeField] Transform displayPoint;
    [SerializeField]
    private GameObject rotLButton;
    [SerializeField]
    private GameObject rotRButton;
    float turningTime;
    [SerializeField] float speed;
    Vector3 currentAngle;
    Vector3 targetRot;
    List<string> robots;
    string robot;
    [SerializeField] private int multiplayerSceneIndex;
    private float timerToStartGame;
    [SerializeField]
    private float maxWaitTime;
    PhotonView PV;
    [SerializeField]
    private Text timerToStartDisplay;
    public PlayerStatistics localPlayerData;
    private bool startingGame;
    private bool launchingLevel;
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

    // Start is called before the first frame update
    void Start()
    {
        localPlayerData = new PlayerStatistics();
        robots = new List<string> { "Balance_Robot", "Attack_Robot", "Defense_Robot", "Speed_Robot"};
        robot = "Balance_Robot";
        DisplayCharacters();
        timerToStartGame = maxWaitTime;
    
        PV = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
            PV.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
    }

    private void Update()
    {
        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, targetRot.y, turningTime), 0);
        this.transform.eulerAngles = currentAngle;

        CountdownToStart();
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
    }

    private void CountdownToStart()
    {
        timerToStartGame -= Time.deltaTime;

        if (timerToStartGame <= 0f)
        {
            timerToStartDisplay.text = "Entrando a partida en " + string.Format("{0:00}", timerToStartGame + 3);

            if (!startingGame)
                StartGame();
            else
            {
                if(timerToStartGame <= -3f && !launchingLevel)
                    LaunchLevel();
            }
        } else
        {
            timerToStartDisplay.text = string.Format("{0:00}", timerToStartGame);
        }
    }

    void rotateTo(float angle)
    {
        targetRot = targetRot + new Vector3(0, angle, 0);
        turningTime = Time.deltaTime * speed;
    }

    private void DisplayCharacters()
    {
        var i = 0;
        foreach (string robot in robots)
        {
            Vector3 baseDir = new Vector3(0, 0, -1);
            Vector3 localDir = Quaternion.Euler(0, i * 360 / robots.Count,0) * baseDir;

            Vector3 offset = localDir - displayPoint.position;

            Vector3 displayPos = displayPoint.position + localDir.normalized * robots.Count * 0.75f;
            GameObject bot = Resources.Load<GameObject>("PhotonPrefabs/"+robot+"_Head");
            GameObject model = Instantiate(bot, displayPos, Quaternion.LookRotation(localDir));
            model.transform.SetParent(this.transform);
            i++;
        }

    }

    public void rotLBtn()
    {
        rotateTo(-360 / robots.Count);
        var index = robots.IndexOf(robot);
        if (index != (robots.Count - 1))
        {
            robot = robots[index + 1];
        }
        else
        {
            robot = robots[(robots.Count - 1) - index];
        }
    }

    public void rotRBtn()
    {
        rotateTo(360/robots.Count);
        var index = robots.IndexOf(robot);
        if (index != 0)
        {
            robot = robots[index - 1];
        }
        else
        {
            robot = robots[(robots.Count - 1) - index];
        }
    }

    public void SelectBtn()
    {
        Button selectBtn = GameObject.FindGameObjectWithTag("SelectButton").GetComponent<Button>();

        switch(LeanLocalization.CurrentLanguage)
        {
            case "Spanish":
            if (selectBtn.GetComponentInChildren<Text>().text == "Elegir")
                selectBtn.GetComponentInChildren<Text>().text = "Cancelar";
            else
                selectBtn.GetComponentInChildren<Text>().text = "Elegir";
                break;
            case "English":
                if (selectBtn.GetComponentInChildren<Text>().text == "Select")
                    selectBtn.GetComponentInChildren<Text>().text = "Cancel";
                else
                    selectBtn.GetComponentInChildren<Text>().text = "Select";
                break;
        }

        foreach (GameObject button in GameObject.FindGameObjectsWithTag("RotButton"))
        {
            buttonEnabled(button, !button.GetComponent<Button>().interactable);
        }
    }

    private void buttonEnabled(GameObject button, bool enabled)
    {
        button.GetComponent<Button>().interactable = enabled;
    }

    public void StartGame()
    {
        startingGame = true;
        FillPlayerData();
        foreach (GameObject button in GameObject.FindGameObjectsWithTag("RotButton"))
        {
            buttonEnabled(button, false);
        }

        Destroy(GameObject.FindGameObjectWithTag("SelectButton"));

        /*
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        */
    }

    private void LaunchLevel()
    {
        launchingLevel = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    private void FillPlayerData()
    {
        switch (robot)
        {
            case "Balance_Robot":
                localPlayerData.model = robots[0];
                localPlayerData.impact = 2.0f;
                localPlayerData.endurance = 2.0f;
                localPlayerData.movementSpeed = 2.0f;
                break;
            case "Attack_Robot":
                localPlayerData.model = robots[1];
                localPlayerData.impact = 3.0f;
                localPlayerData.endurance = 1.0f;
                localPlayerData.movementSpeed = 2.0f;
                break;
            case "Defense_Robot":
                localPlayerData.model = robots[2];
                localPlayerData.impact = 2.0f;
                localPlayerData.endurance = 3.0f;
                localPlayerData.movementSpeed = 1.0f;
                break;
            case "Speed_Robot":
                localPlayerData.model = robots[3];
                localPlayerData.impact = 1.0f;
                localPlayerData.endurance = 2.0f;
                localPlayerData.movementSpeed = 3.0f;
                break;
        }
        localPlayerData.playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        GlobalControl.Instance.savedPlayerData = localPlayerData;
    }
}
