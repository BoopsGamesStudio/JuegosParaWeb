using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject myConePrefab;
    [SerializeField]
    private GameObject someoneElsesConePrefab;

    private PhotonView PV;

    public enum cornerNames { North, East, South, West }

    public enum sides { Left, Center, Right }

    //[SerializeField] List<Item> inventory;
    //[SerializeField] float movementSpeed;
    [SerializeField] float rotSpeed;
    [HideInInspector] public float x;
    [HideInInspector] public float z;
    //[HideInInspector] public cornerNames previousCorner;
    [HideInInspector] public cornerNames currentCorner;
    private float cameraAnlgeOffset = -45;
    [HideInInspector]
    public bool cameraHorizontal = false;

    Canvas mainCanvas;

    private GameObject PhoneInputs;
    private Joystick joystick;
    private Button LButton;
    private Button RButton;

    private Vector3 rot;

    private GameObject weaponInTrigger;
    private GameObject[] stageElems;
    private Camera cam;
    private bool alreadyTeleported;
    private float cooldown;
    Animator anim;
    float timerForAnim;
    float timerForStun;
    bool stunned = false;

    bool gameOver;
    bool dead;
    bool scoreDisplayed;

    GameObject cone;

    [SerializeField] private GameObject buttonPrefab;

    List<int> leaderboard = new List<int>();

    #region Stats
    /*
    private float impact = 2;
    private float movementSpeed = 2;
    private float endurance = 2;
    */
    public PlayerStatistics localPlayerData;
    #endregion

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "SearchLevel")
            currentCorner = (cornerNames)PhotonNetwork.LocalPlayer.ActorNumber - 1;
        else
            currentCorner = cornerNames.South;

        PV = GetComponent<PhotonView>();

        anim = GetComponent<Animator>();

        localPlayerData = new PlayerStatistics();
        if (PV.IsMine)
        {
            initPlayerStats();
            cone = Instantiate(myConePrefab, this.transform);
        } else
        {
            cone = Instantiate(someoneElsesConePrefab, this.transform);
        }

        cone.transform.localScale = new Vector3(5, 5, 5);
        cone.transform.localPosition = new Vector3(0, 2, 0);

        PhoneInputs = GameObject.FindGameObjectWithTag("PhoneInputs");

        if (!Application.isMobilePlatform)
        {
            if (PhoneInputs != null)
                GameObject.Destroy(PhoneInputs.gameObject);
        }
        else
        {
            joystick = PhoneInputs.GetComponentInChildren<Joystick>();

            if (SceneManager.GetActiveScene().name == "SearchLevel")
            {
                foreach (Button button in PhoneInputs.GetComponentsInChildren<Button>())
                {
                    if (button.gameObject.name == "LButton")
                        LButton = button;
                    if (button.gameObject.name == "RButton")
                        RButton = button;
                }
            }
            else
            {
                foreach (Button button in PhoneInputs.GetComponentsInChildren<Button>())
                {
                    if (button.gameObject.name == "LButton")
                        Destroy(button.gameObject);
                    if (button.gameObject.name == "RButton")
                        RButton = button;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            foreach (Canvas canv in FindObjectsOfType<Canvas>())
            {
                if (canv.name == "AltCanvas")
                    continue;

                mainCanvas = canv;
            }

            stageElems = GameObject.FindGameObjectsWithTag("stage");
            cam = FindObjectOfType<Camera>().GetComponent<Camera>();

            if (SceneManager.GetActiveScene().name != "SearchLevel")
            {
                if (GlobalControl.Instance.savedPlayerData.inventory.Exists((x) => x is Weapon))
                {
                    createWeaponIcon(GlobalControl.Instance.savedPlayerData.getWeapon().getName(), new Vector2(125, 115), false, false);
                }
            }

            if (Application.isMobilePlatform)
            {
                if (SceneManager.GetActiveScene().name == "SearchLevel")
                {
                    LButton.onClick.AddListener(cam.transform.parent.GetComponent<CameraRotation>().pressL);
                    RButton.onClick.AddListener(cam.transform.parent.GetComponent<CameraRotation>().pressR);
                }
                else
                {
                    RButton.onClick.AddListener(attackButton);

                    mainCanvas.transform.Find("CharacterFrame").GetComponent<RectTransform>().anchoredPosition = new Vector2(140, -180);
                    mainCanvas.transform.Find("CharacterFrame").GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -90));

                    mainCanvas.transform.Find("WeaponFrame").GetComponent<RectTransform>().anchoredPosition = new Vector2(130, -140);
                    mainCanvas.transform.Find("WeaponFrame").GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -90));

                    if (GlobalControl.Instance.savedPlayerData.inventory.Exists((x) => x is Weapon))
                    {
                        mainCanvas.transform.Find("WeaponIcon").GetComponent<RectTransform>().anchoredPosition = new Vector2(130, -140); //Pos y rot mal
                        mainCanvas.transform.Find("WeaponIcon").GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -90));
                    }

                    mainCanvas.transform.Find("PlayerIcon").GetComponent<RectTransform>().anchoredPosition = new Vector2(140, -140);
                    mainCanvas.transform.Find("PlayerIcon").GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -90));
                }
            }

            mainCanvas.transform.Find("PlayerIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + GlobalControl.Instance.savedPlayerData.model);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (gameObject.CompareTag("Player"))
            {
                switch (currentCorner)
                {
                    case cornerNames.West:
                        cameraAnlgeOffset = 45;
                        break;
                    case cornerNames.South:
                        cameraAnlgeOffset = -45;
                        break;
                    case cornerNames.North:
                        cameraAnlgeOffset = 135;
                        break;
                    case cornerNames.East:
                        cameraAnlgeOffset = -135;
                        break;
                }

                if (!stunned)
                {
                    if (joystick != null)
                    {
                        Vector3 vel;
                        
                        if (SceneManager.GetActiveScene().name == "SearchLevel")
                            vel = new Vector3(-joystick.Horizontal, gameObject.GetComponent<Rigidbody>().velocity.y, -joystick.Vertical);
                        else
                            vel = new Vector3(joystick.Vertical, gameObject.GetComponent<Rigidbody>().velocity.y, -joystick.Horizontal);

                        vel = Quaternion.AngleAxis(cameraAnlgeOffset, Vector3.up) * vel;

                        Vector2 velocity2D = new Vector2(vel.x, vel.z).normalized;
                        if (velocity2D.magnitude > 0.1f)
                        {
                            anim.SetBool("isWalking", true);
                            this.transform.rotation = Quaternion.LookRotation(new Vector3(velocity2D.x, 0, velocity2D.y));
                            transform.Translate(0, 0, velocity2D.magnitude * Time.deltaTime * localPlayerData.movementSpeed);
                        }
                        else
                        {
                            anim.SetBool("isWalking", false);
                        }
                    }
                    else
                    {
                        z = Input.GetAxis("Vertical") * Time.deltaTime * localPlayerData.movementSpeed;

                        float speedReduceFactor = z <= 0 ? 1 : 0.5f;
                        x = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed * speedReduceFactor;

                        if (z > 0f)
                        {
                            anim.SetBool("isWalking", true);
                        }
                        else
                        {
                            anim.SetBool("isWalking", false);
                            //z *= 0.35f;
                        }

                        transform.Rotate(0, x, 0);
                        transform.Translate(0, 0, z);
                    }
                }
            }

            if (SceneManager.GetActiveScene().name == "BattleScene1" || SceneManager.GetActiveScene().name == "BattleScene2" || SceneManager.GetActiveScene().name == "BattleScene3")
            {
                cooldown -= Time.deltaTime;
                if (!stunned)
                {
                    if(RButton != null) RButton.interactable = true;
                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        attackButton();
                    }
                } else
                {
                    if (RButton != null) RButton.interactable = false;
                }

                if (localPlayerData.inventory.Exists((x) => x is Weapon))
                {
                    if (cooldown < localPlayerData.getWeapon().getCadence() - 0.2f)
                    {
                        anim.SetBool("attack", false);
                    }
                } else
                {
                    if(cooldown < -0.2f)
                        anim.SetBool("attack", false);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("My stats: " + localPlayerData.getStats());
                Debug.Log("Inv local: ");
                foreach (Item i in localPlayerData.inventory)
                {
                    Debug.Log(i.getAttribs());
                }

                Debug.Log("Inv global: ");
                foreach (Item i in GlobalControl.Instance.savedPlayerData.inventory)
                {
                    Debug.Log(i.getAttribs());
                }
            }

            if (anim.GetBool("isHitted"))
            {
                timerForAnim += Time.deltaTime;

                if(timerForAnim > 0.2f)
                {
                    anim.SetBool("isHitted", false);
                    timerForAnim = 0;
                }
            }

            if(stunned)
            {
                timerForStun += Time.deltaTime;

                if (timerForStun > 2) //Duración del stun
                {
                    stunned = false;
                    timerForStun = 0;
                }
            }

            if (!gameOver)
            {
                if (this.transform.position.y < -20 && !dead)
                {
                    PV.RPC("RPC_Die", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
                    mainCanvas.GetComponentInChildren<Text>().text = "<color=#a10b00><b>YOU LOSE</b></color>";
                    dead = true;
                }
            } else
            {
                if (!scoreDisplayed) {
                    if (!dead)
                    {
                        mainCanvas.GetComponentInChildren<Text>().text = "<color=#00991f><b>YOU WIN</b></color>";
                        leaderboard.Add(PhotonNetwork.LocalPlayer.ActorNumber);
                        scoreDisplayed = true;
                    }
                    else
                    {
                        foreach (Player p in PhotonNetwork.PlayerList)
                        {
                            if (!leaderboard.Contains(p.ActorNumber))
                            {
                                leaderboard.Add(p.ActorNumber);
                                break;
                            }
                        }
                    }

                    string[] score = new string[leaderboard.Count];

                    for (int i = 0; i < score.Length; i++)
                    {
                        score[i] = PhotonNetwork.PlayerList[leaderboard[score.Length - i - 1] - 1].ActorNumber.ToString();
                    }

                    string scoreTxt = "FINAL LEADERBOARD";

                    foreach (string elem in score)
                    {
                        if (PhotonNetwork.LocalPlayer.ActorNumber.ToString() == elem)
                            scoreTxt += "\n<color=#00991f><b>- Player " + elem + "</b></color>";
                        else
                            scoreTxt += "\n- Player " + elem;
                    }

                    mainCanvas.transform.Find("ScorePanel").gameObject.SetActive(true);
                    mainCanvas.transform.Find("ScorePanel").GetComponentInChildren<Text>().text = scoreTxt;
                }
            }
        }
    }

    [PunRPC]
    void RPC_Die(int player)
    {
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            Debug.Log("player " + player + " died haha");
            playerObject.GetComponent<PlayerController>().leaderboard.Add(player);

            if (playerObject.GetComponent<PlayerController>().leaderboard.Count >= PhotonNetwork.PlayerList.Length - 1)
            {
                playerObject.GetComponent<PlayerController>().gameOver = true;
            }
        }
    }

    public void attackButton()
    {
        if (cooldown <= 0)
        {
            cooldown = 1;
            if (localPlayerData.inventory.Exists((x) => x is Weapon))
                cooldown = localPlayerData.getWeapon().getCadence();

            if (localPlayerData.inventory.Exists((x) => x is Weapon) && localPlayerData.getWeaponType() == Weapon.weaponType.Distance)
            {
                gunShoot();
            }
            else
            {
                meleeHit();
            }

            anim.SetBool("attack", true);
        }
    }
    
    private void meleeHit()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (gameObject.GetComponentInChildren<BoxCollider>().bounds.Contains(o.transform.position) && !o.Equals(gameObject))
            {
                Debug.Log("MELEE HIT with impact: " + localPlayerData.impact);

                Vector3 impactVector = this.transform.forward;
                impactVector.y = 0.5f;

                Vector3 force = 0.03f * impactVector.normalized * localPlayerData.impact;
                bool usingMelee;
                if(localPlayerData.inventory.Exists((x) => x is Weapon))
                {
                    usingMelee = localPlayerData.getWeaponType() == Weapon.weaponType.Melee;
                } else
                {
                    usingMelee = false;
                }

                PV.RPC("RPC_Hit", o.GetComponent<PhotonView>().Owner, force, o.GetComponent<PhotonView>().Owner.ActorNumber, usingMelee);
            }
        }
    }

    private void gunShoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), this.transform.position, this.transform.rotation);
        bullet.GetComponent<Bullet>().setImpact(localPlayerData.impact);
    }

    [PunRPC]
    private void RPC_Hit(Vector3 force, int player, bool usingMelee)
    {
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(GO.GetComponent<PhotonView>().Owner.ActorNumber == player)
            {
                GO.GetComponent<Rigidbody>().AddForce(force / GO.GetComponent<PlayerController>().localPlayerData.endurance, ForceMode.Impulse);

                GO.GetComponent<PlayerController>().stunned = true;
                GO.GetComponent<Animator>().SetBool("enemyUsesMelee", usingMelee);
                GO.GetComponent<Animator>().SetBool("isHitted", true);
            }
        }
    }

    [PunRPC]
    private void RPC_LoadWeapon(int player, int weapon)
    {
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (GO.GetComponent<PhotonView>().Owner.ActorNumber == player)
            {
                GO.transform.GetChild(17).gameObject.SetActive(false);
                GO.transform.GetChild(weapon).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (PV.IsMine)
        {
            string goodName;
            switch (col.gameObject.tag)
            {
                case "buff":
                    goodName = col.gameObject.name.Replace("(Clone)", "");
                    localPlayerData.inventory.Add(new BuffItem(goodName));
                    PV.RPC("RPC_DestroyObject", RpcTarget.MasterClient, col.gameObject.GetPhotonView().ViewID);
                    //RPC_DestroyObject(col.gameObject);
                    break;
                case "weapon":
                    weaponInTrigger = col.gameObject;
                    goodName = weaponInTrigger.name.Replace("(Clone)", "");

                    if (!localPlayerData.inventory.Exists((x) => x is Weapon))
                    {
                        createWeaponIcon(goodName, new Vector2(90, 50), true, false);
                        localPlayerData.inventory.Add(new Weapon(goodName));
                        PV.RPC("RPC_DestroyObject", RpcTarget.MasterClient, col.gameObject.GetPhotonView().ViewID);
                        //RPC_DestroyObject(col.gameObject);
                    }
                    else
                    {
                        createButton(goodName);
                    }
                    break;
                case "consumable":
                    goodName = col.gameObject.name.Replace("(Clone)", "");
                    localPlayerData.inventory.Add(new ConsumableItem(goodName));
                    PV.RPC("RPC_DestroyObject", RpcTarget.MasterClient, col.gameObject.GetPhotonView().ViewID);
                    //RPC_DestroyObject(col.gameObject);
                    break;
                case "Teleporter":
                    switch (col.gameObject.name)
                    {
                        case "teleporter1a":
                            if (!alreadyTeleported)
                            {
                                transform.position = GameObject.Find("teleporter1b").transform.position;
                                transform.rotation = GameObject.Find("teleporter1b").transform.rotation;
                            }
                            break;
                        case "teleporter1b":
                            if (!alreadyTeleported)
                            {
                                transform.position = GameObject.Find("teleporter1a").transform.position;
                                transform.rotation = GameObject.Find("teleporter1a").transform.rotation;
                            }
                            break;
                        case "teleporter2a":
                            if (!alreadyTeleported)
                            {
                                transform.position = GameObject.Find("teleporter2b").transform.position;
                                transform.rotation = GameObject.Find("teleporter2b").transform.rotation;
                            }
                            break;
                        case "teleporter2b":
                            if (!alreadyTeleported)
                            {
                                transform.position = GameObject.Find("teleporter2a").transform.position;
                                transform.rotation = GameObject.Find("teleporter2a").transform.rotation;
                            }
                            break;
                    }

                    break;
                case "CamTrigger":
                    cameraHorizontal = true;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PV.IsMine)
        {
            foreach (GameObject swapButton in GameObject.FindGameObjectsWithTag("swapButton"))
            {
                Destroy(swapButton);
                createWeaponIcon(localPlayerData.getWeapon().getName(), new Vector2(90, 50), false, true);
            }

            if (other.gameObject.CompareTag("Teleporter"))
                alreadyTeleported = !alreadyTeleported;

            if (other.gameObject.CompareTag("CamTrigger"))
                cameraHorizontal = false;
        }
    }

    public void initPlayerStats()
    {
        if (SceneManager.GetActiveScene().name == "SearchLevel")
        {
            localPlayerData = GlobalControl.Instance.savedPlayerData;
            localPlayerData.inventory = new List<Item>();
        }
        if (SceneManager.GetActiveScene().name == "BattleScene1" || SceneManager.GetActiveScene().name == "BattleScene2" || SceneManager.GetActiveScene().name == "BattleScene3")
        {
            localPlayerData = GlobalControl.Instance.savedPlayerData;
            //localPlayerData.inventory = GlobalControl.Instance.savedPlayerData.inventory;

            if (localPlayerData.inventory.Exists((x) => x is Weapon))
            {
                switch (localPlayerData.getWeaponType())
                {
                    case Weapon.weaponType.Distance:
                        anim.SetInteger("weaponType", 2);
                        break;
                    case Weapon.weaponType.Melee:
                        anim.SetInteger("weaponType", 1);
                        break;
                    case Weapon.weaponType.Shield:
                        anim.SetInteger("weaponType", 3);
                        break;
                }
            }
        }
    }

    public int getPlayerId()
    {
        return localPlayerData.playerId;
    }

    public void setPlayerName(int playerId)
    {
        localPlayerData.playerId = playerId;
    }

    public void savePlayer()
    {
        //Save Player Data

        //Update Player Data with caught objects
        foreach (Item i in GlobalControl.Instance.savedPlayerData.inventory)
        {
            if (i is BuffItem)
            {
                var buff = (BuffItem)i;
                switch (buff.getType())
                {
                    case BuffItem.buffType.Impact:
                        GlobalControl.Instance.savedPlayerData.impact += buff.getBuff();
                        break;
                    case BuffItem.buffType.Endurance:
                        GlobalControl.Instance.savedPlayerData.endurance += buff.getBuff();
                        break;
                    case BuffItem.buffType.Speed:
                        GlobalControl.Instance.savedPlayerData.movementSpeed += buff.getBuff();
                        break;
                }

            }

            if (i is Weapon)
            {
                var weapon = (Weapon) i;
                
                GlobalControl.Instance.savedPlayerData.impact += weapon.getImpact();
                GlobalControl.Instance.savedPlayerData.endurance += weapon.getEndurance();
                GlobalControl.Instance.savedPlayerData.movementSpeed += weapon.getSpeed();
            }
        }

        Debug.Log(GlobalControl.Instance.savedPlayerData.getStats());

    }

    void createButton(string goodName)
    {
        GameObject button = Instantiate(buttonPrefab, mainCanvas.transform);
        button.GetComponent<Button>().onClick.AddListener(replaceWeapon);

        button.GetComponentInChildren<Text>().GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        button.GetComponentInChildren<Text>().GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);

        button.GetComponentInChildren<Text>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 120);
        button.GetComponentInChildren<Text>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90);

        button.GetComponentInChildren<Text>().text = "<b>-></b>";
        button.GetComponentInChildren<Text>().fontSize = 60;

        button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 80);
        button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60);

        createWeaponIcon(goodName, new Vector2(-90, 50), false, false);
    }

    void createWeaponIcon(string name, Vector2 pos, bool overrideImg, bool deleteAllPrevious)
    {
        if(deleteAllPrevious)
            clearWeaponIcon();

        if (overrideImg)
        {
            foreach (Image ico in mainCanvas.GetComponentsInChildren<Image>())
            {
                if (ico.gameObject.CompareTag("item"))
                {
                    ico.overrideSprite = Resources.Load<Sprite>("Icons/" + name);
                    return;
                }
            }
        }

        GameObject icon = new GameObject("WeaponIcon");
        icon.tag = "item";
        Image imgComp = icon.AddComponent<Image>();
        imgComp.sprite = Resources.Load<Sprite>("Icons/" + name);

        icon.transform.SetParent(mainCanvas.transform);
        //icon.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
        icon.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    void clearWeaponIcon()
    {
        foreach (Image icon in mainCanvas.GetComponentsInChildren<Image>())
        {
            if (icon.gameObject.CompareTag("item"))
            {
                Destroy(icon.gameObject);
            }
        }
    }

    public void replaceWeapon()
    {
        string goodName = weaponInTrigger.name.Replace("(Clone)", "");
        createWeaponIcon(goodName, new Vector2(90, 50), false, true);
        localPlayerData.inventory.RemoveAll((x) => x is Weapon);
        localPlayerData.inventory.Add(new Weapon(goodName));
        PV.RPC("RPC_DestroyObject", RpcTarget.MasterClient, weaponInTrigger.GetPhotonView().ViewID);

        foreach (GameObject swapButton in GameObject.FindGameObjectsWithTag("swapButton"))
        {
            Destroy(swapButton);
        }
    }

    /*
    private void raycastTest()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(cam.WorldToScreenPoint(this.gameObject.transform.position)), out hitInfo);
        if (hit)
        {
            if (!hitInfo.transform.gameObject.Equals(this.gameObject) && hitInfo.transform.gameObject.CompareTag("stage"))
            {
                cameraHorizontal = true;
            }
            else
            {
                cameraHorizontal = false;
            }
        }
    }*/

    [PunRPC]
    private void RPC_DestroyObject(int viewID)
    {
        PhotonNetwork.Destroy(PhotonNetwork.GetPhotonView(viewID));
    }
}
