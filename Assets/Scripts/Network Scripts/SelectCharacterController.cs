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
    [SerializeField]
    private GameObject StartButton;
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
    private bool startingGame;

    // Start is called before the first frame update
    void Start()
    {
        robots = new List<string> { "cabeza_equilibrio", "cabeza_ataque", "cabeza_defensa", "cabeza_velocidad"};
        robot = "cabeza_equilibrio";
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

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if (timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
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
            GameObject bot = Resources.Load<GameObject>("PhotonPrefabs/"+robot);
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

        if(selectBtn.GetComponentInChildren<Text>().text == "Select")
            selectBtn.GetComponentInChildren<Text>().text = "Cancel";
        else
            selectBtn.GetComponentInChildren<Text>().text = "Select";

        foreach (GameObject button in GameObject.FindGameObjectsWithTag("RotButton"))
        {
            button.GetComponent<Button>().interactable = !button.GetComponent<Button>().interactable;
        }
    }

    public void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }
}
