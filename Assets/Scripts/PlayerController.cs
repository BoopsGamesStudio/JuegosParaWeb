using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PhotonView PV;

    public enum cornerNames { North, East, South, West }

    public enum sides { Left, Center, Right }

    //[SerializeField] List<Item> inventory;
    //[SerializeField] float movementSpeed;
    [SerializeField] float rotSpeed;
    [HideInInspector] public float x;
    [HideInInspector] public float z;
    //[HideInInspector] public cornerNames previousCorner;
    [HideInInspector] public cornerNames currentCorner = cornerNames.South;//Modificar para cada jugador
    [HideInInspector] public sides currentSide = sides.Center;
    private float cameraAnlgeOffset = -45;
    public bool cameraHorizontal = false;

    private Joystick joystick;
    private Vector3 rot;

    private GameObject weaponInTrigger;
    private GameObject[] stageElems;
    private Camera cam;
    private bool alreadyTeleported;

    [SerializeField] private GameObject buttonPrefab;

    #region Stats
    /*
    private float impact = 2;
    private float movementSpeed = 2;
    private float endurance = 2;
    */
    public PlayerStatistics localPlayerData;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        Debug.Log("Start");
        localPlayerData = new PlayerStatistics();
        //GlobalControl.Instance.savedPlayerData = new List<PlayerStatistics>();
        if(PV.IsMine)
        initPlayerStats();

        joystick = FindObjectOfType<Joystick>();
        stageElems = GameObject.FindGameObjectsWithTag("stage");
        cam = FindObjectOfType<Camera>().GetComponent<Camera>();

        if (!Application.isMobilePlatform)
        {
            if (joystick != null)
                GameObject.Destroy(joystick.gameObject);
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

                if (joystick != null)
                {
                    Vector3 vel;

                    if (SceneManager.GetActiveScene().name == "Scene2")
                    {
                        vel = new Vector3(joystick.Vertical, gameObject.GetComponent<Rigidbody>().velocity.y, -joystick.Horizontal);
                    }
                    else
                    {
                        vel = new Vector3(-joystick.Horizontal , gameObject.GetComponent<Rigidbody>().velocity.y, -joystick.Vertical);
                    }

                    vel = Quaternion.AngleAxis(cameraAnlgeOffset, Vector3.up) * vel;

                    //gameObject.GetComponent<Rigidbody>().velocity = vel;

                    Vector2 velocity2D = new Vector2(vel.x, vel.z).normalized;
                    if (velocity2D.magnitude > 0.1f)
                    {
                        this.transform.rotation = Quaternion.LookRotation(new Vector3(velocity2D.x, 0, velocity2D.y));
                        //this.transform.Translate(velocity2D.x * Time.deltaTime * localPlayerData.movementSpeed,0 ,velocity2D.y * Time.deltaTime * localPlayerData.movementSpeed);
                        transform.Translate(0, 0, velocity2D.magnitude * Time.deltaTime * localPlayerData.movementSpeed);
                    }
                }
                else
                {
                    x = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed;
                    z = Input.GetAxis("Vertical") * Time.deltaTime * localPlayerData.movementSpeed;

                    transform.Rotate(0, x, 0);
                    transform.Translate(0, 0, z);
                }
            }

            if (SceneManager.GetActiveScene().name == "Scene2")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (gameObject.GetComponentInChildren<BoxCollider>().bounds.Contains(o.transform.position) && !o.Equals(gameObject))
                        {
                            Vector3 impactVector = this.transform.forward;
                            impactVector.y = 0.5f;
                            
                            Vector3 force = 0.04f * impactVector.normalized * localPlayerData.impact / o.GetComponent<PlayerController>().localPlayerData.endurance;
                            PV.RPC("RPC_Hit", o.GetComponent<PhotonView>().Owner, force, o.GetComponent<PhotonView>().Owner.ActorNumber);
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (Item i in localPlayerData.inventory)
                {
                    Debug.Log(i.getAttribs());
                }
            }

            if (SceneManager.GetActiveScene().name == "Scene1" || SceneManager.GetActiveScene().name == "Level2" || SceneManager.GetActiveScene().name == "Level3")
            {
                //raycastTest();
            }
        }


    }
    
    [PunRPC]
    private void RPC_Hit(Vector3 force, int player)
    {
        foreach(GameObject GO in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(GO.GetComponent<PhotonView>().Owner.ActorNumber == player)
            {
                GO.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            }
        }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        string goodName;
        switch (col.gameObject.tag)
        {
            case "buff":
                goodName = col.gameObject.name.Replace("(Clone)", "");
                localPlayerData.inventory.Add(new BuffItem(goodName));
                //PV.RPC("RPC_DestroyObject", RpcTarget.MasterClient, col.gameObject);
                RPC_DestroyObject(col.gameObject);
                break;
            case "weapon":
                weaponInTrigger = col.gameObject;
                goodName = weaponInTrigger.name.Replace("(Clone)", "");

                if (!localPlayerData.inventory.Exists((x) => x is Weapon))
                {
                    createText(goodName, new Vector2(300, 600));
                    localPlayerData.inventory.Add(new Weapon(goodName));
                    //PV.RPC("RPC_DestroyObject", RpcTarget.MasterClient, col.gameObject);
                    RPC_DestroyObject(col.gameObject);
                }
                else
                {
                    createButton("¿Cambiar por " + goodName + "?");
                }
                break;
            case "consumable":
                goodName = col.gameObject.name.Replace("(Clone)", "");
                localPlayerData.inventory.Add(new ConsumableItem(goodName));
                //PV.RPC("RPC_DestroyObject", RpcTarget.MasterClient, col.gameObject);
                RPC_DestroyObject(col.gameObject);
                break;
            case "Teleporter":
                switch (col.gameObject.name)
                {
                    case "teleporter1a":
                        if(!alreadyTeleported)
                        transform.position = GameObject.Find("teleporter1b").transform.position;
                        break;
                    case "teleporter1b":
                        if (!alreadyTeleported)
                            transform.position = GameObject.Find("teleporter1a").transform.position;
                        break;
                    case "teleporter2a":
                        if (!alreadyTeleported)
                            transform.position = GameObject.Find("teleporter2b").transform.position;
                        break;
                    case "teleporter2b":
                        if (!alreadyTeleported)
                            transform.position = GameObject.Find("teleporter2a").transform.position;
                        break;
                }
                
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        foreach (GameObject swapButton in GameObject.FindGameObjectsWithTag("swapButton"))
        {
            Destroy(swapButton);
        }
        if (other.gameObject.CompareTag("Teleporter"))
            alreadyTeleported = !alreadyTeleported;
    }

    public void initPlayerStats()
    {
        if (SceneManager.GetActiveScene().name == "Scene1" || SceneManager.GetActiveScene().name == "Level2" || SceneManager.GetActiveScene().name == "Level3")
        {
            Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
            localPlayerData = GlobalControl.Instance.savedPlayerData.Find((x) => x.playerId == PhotonNetwork.LocalPlayer.ActorNumber);
            localPlayerData.inventory = new List<Item>();
        }
        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            localPlayerData = GlobalControl.Instance.savedPlayerData.Find((x) => x.playerId == PhotonNetwork.LocalPlayer.ActorNumber);
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
        var index = GlobalControl.Instance.savedPlayerData.IndexOf(localPlayerData);

        /*
        foreach (PlayerStatistics ps in GlobalControl.Instance.savedPlayerData)
        {
            Debug.Log(ps.getStats());
        }
        */

        //Update Player Data with caught objects
        foreach (Item i in GlobalControl.Instance.savedPlayerData[index].inventory)
        {
            if (i is BuffItem)
            {
                var buff = (BuffItem)i;
                switch (buff.getType())
                {
                    case BuffItem.buffType.Impact:
                        GlobalControl.Instance.savedPlayerData[index].impact += buff.getBuff();
                        break;
                    case BuffItem.buffType.Endurance:
                        GlobalControl.Instance.savedPlayerData[index].endurance += buff.getBuff();
                        break;
                    case BuffItem.buffType.Speed:
                        GlobalControl.Instance.savedPlayerData[index].movementSpeed += buff.getBuff();
                        break;
                }

            }

            if (i is Weapon)
            {
                var weapon = (Weapon)i;
                    
               GlobalControl.Instance.savedPlayerData[index].impact += weapon.getImpact();
                        
                    
               GlobalControl.Instance.savedPlayerData[index].endurance += weapon.getEndurance();
                        
       
               GlobalControl.Instance.savedPlayerData[index].movementSpeed += weapon.getSpeed();
                       

            }
        }

        foreach (PlayerStatistics ps in GlobalControl.Instance.savedPlayerData)
        {
            Debug.Log(ps.getStats());
        }

    }

    void createButton(string txt)
    {
        GameObject button = Instantiate(buttonPrefab, FindObjectOfType<Canvas>().transform);
        button.GetComponent<Button>().onClick.AddListener(replaceWeapon);
        button.GetComponentInChildren<Text>().text = txt;
    }

    void createText(string txt, Vector2 pos)
    {
        clearText();

        GameObject text = new GameObject();
        text.tag = "item";
        Text textComp = text.AddComponent<Text>();
        textComp.text = txt;
        textComp.font = Resources.Load<Font>("ARIAL");
        textComp.fontSize = 50;
        textComp.alignment = TextAnchor.MiddleCenter;

        text.transform.SetParent(FindObjectOfType<Canvas>().transform);
        text.GetComponent<RectTransform>().localScale = Vector3.one;
        text.GetComponent<RectTransform>().anchoredPosition = pos;
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
    }

    void clearText()
    {
        foreach (Text t in FindObjectOfType<Canvas>().GetComponentsInChildren<Text>())
        {
            if (t.gameObject.CompareTag("item"))
            {
                Destroy(t.gameObject);
            }
        }
    }

    public void replaceWeapon()
    {
        string goodName = weaponInTrigger.name.Replace("(Clone)", "");
        createText(goodName, new Vector2(300, 600));
        localPlayerData.inventory.RemoveAll((x) => x is Weapon);
        localPlayerData.inventory.Add(new Weapon(goodName));
        PV.RPC("RPC_DestroyObject", RpcTarget.MasterClient, weaponInTrigger);

        foreach (GameObject swapButton in GameObject.FindGameObjectsWithTag("swapButton"))
        {
            Destroy(swapButton);
        }
    }

    //cambiar a checkear un trigger
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
    }

    [PunRPC]
    private void RPC_DestroyObject(GameObject g)
    {
        PhotonNetwork.Destroy(g.GetPhotonView());
    }
}
