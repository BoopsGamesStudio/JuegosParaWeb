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

    //[SerializeField] List<Item> inventory;
    //[SerializeField] float movementSpeed;
    [SerializeField] float rotSpeed;
    [HideInInspector] public float x;
    [HideInInspector] public float z;
    //[HideInInspector] public cornerNames previousCorner;
    [HideInInspector] public cornerNames currentCorner = cornerNames.South;
    private float cameraAnlgeOffset = -45;

    private Joystick joystick;
    private Vector3 rot;

    public GameObject weaponInTrigger;
    private GameObject[] stageElems;
    private Camera cam;

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
        initPlayerStats();

        joystick = FindObjectOfType<Joystick>();
        stageElems = GameObject.FindGameObjectsWithTag("stage");
        cam = FindObjectOfType<Camera>().GetComponent<Camera>();

        if (!Application.isMobilePlatform)
        {
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

                    if (SceneManager.GetActiveScene().name == "Scene1")
                    {
                        vel = new Vector3(-joystick.Horizontal * localPlayerData.movementSpeed, gameObject.GetComponent<Rigidbody>().velocity.y, -joystick.Vertical * localPlayerData.movementSpeed);
                    }
                    else
                    {
                        vel = new Vector3(joystick.Vertical * localPlayerData.movementSpeed, gameObject.GetComponent<Rigidbody>().velocity.y, -joystick.Horizontal * localPlayerData.movementSpeed);
                    }

                    vel = Quaternion.AngleAxis(cameraAnlgeOffset, Vector3.up) * vel;

                    gameObject.GetComponent<Rigidbody>().velocity = vel;

                    Vector2 velocity2D = new Vector2(vel.x, vel.z);
                    if (velocity2D.magnitude > 0.1f)
                    {
                        Vector3 dir = gameObject.GetComponent<Rigidbody>().velocity.normalized;
                        dir.y = 0;
                        this.transform.rotation = Quaternion.LookRotation(dir);
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
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("Dummy"))
                    {
                        if (gameObject.GetComponentInChildren<BoxCollider>().bounds.Contains(o.transform.position) && !o.Equals(gameObject))
                        {
                            Vector3 impactVector = this.transform.forward;
                            impactVector.y = 0.5f;
                            o.GetComponent<Rigidbody>().AddForce(0.04f * impactVector.normalized * localPlayerData.impact / o.GetComponent<PlayerController>().localPlayerData.endurance, ForceMode.Impulse);
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

            if (SceneManager.GetActiveScene().name == "Scene1")
            {
                raycastTest();
            }
        }
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("buff"))
        {
            Debug.Log(col.gameObject.name);
            Destroy(col.gameObject);
            localPlayerData.inventory.Add(new BuffItem(col.gameObject.name));

        }
        if (col.gameObject.CompareTag("weapon"))
        {
            weaponInTrigger = col.gameObject;

            if (!localPlayerData.inventory.Exists((x) => x is Weapon)) {
                Debug.Log(weaponInTrigger.name);
                createText(weaponInTrigger.name, new Vector2(300, 600));
                localPlayerData.inventory.Add(new Weapon(weaponInTrigger.name));
                Destroy(weaponInTrigger);
            } else
            {
                createButton("¿Cambiar por " + col.gameObject.name + "?");
            }

        }
        if (col.gameObject.CompareTag("consumable"))
        {
            Debug.Log(col.gameObject.name);
            Destroy(col.gameObject);
            localPlayerData.inventory.Add(new ConsumableItem(col.gameObject.name));

        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject swapButton in GameObject.FindGameObjectsWithTag("swapButton"))
        {
            Destroy(swapButton);
        }
    }

    /*
    #region Getters & Setters
    public float getImpact()
    {
        return impact;
    }

    public float getMovementSpeed()
    {
        return movementSpeed;
    }

    public float getEndurance()
    {
        return endurance;
    }

    public void setImpact(float impact)
    {
        this.impact = impact;
    }

    public void setMovementSpeed(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    public void setEndurance(float endurance)
    {
        this.endurance = endurance;
    }
    #endregion
    */

    public void initPlayerStats()
    {
        if (SceneManager.GetActiveScene().name == "Scene1")
        {
            localPlayerData.playerId = 0;
            localPlayerData.impact = 2f;
            localPlayerData.endurance = 2f;
            localPlayerData.movementSpeed = 3f;
            localPlayerData.inventory = new List<Item>();
        }
        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            localPlayerData = GlobalControl.Instance.savedPlayerData[0];
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
        GlobalControl.Instance.savedPlayerData.Insert(localPlayerData.playerId, localPlayerData);

        foreach (PlayerStatistics ps in GlobalControl.Instance.savedPlayerData)
        {
            Debug.Log(ps.getStats());
        }

        //Update Player Data with caught objects
        foreach (Item i in GlobalControl.Instance.savedPlayerData[localPlayerData.playerId].inventory)
        {
           if(i is BuffItem)
            {
                var buff = (BuffItem)i;
                switch (buff.getType())
                {
                    case BuffItem.buffType.Impact:
                        GlobalControl.Instance.savedPlayerData[localPlayerData.playerId].impact += buff.getBuff(); 
                        break;
                    case BuffItem.buffType.Endurance:
                        GlobalControl.Instance.savedPlayerData[localPlayerData.playerId].endurance += buff.getBuff();
                        break;
                    case BuffItem.buffType.Speed:
                        GlobalControl.Instance.savedPlayerData[localPlayerData.playerId].movementSpeed += buff.getBuff();
                        break;
                }

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
            if (t.gameObject.CompareTag("item")) {
                Destroy(t.gameObject);
            }
        }
    }

    public void replaceWeapon()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        Debug.Log(player.weaponInTrigger.name);
        createText(player.weaponInTrigger.name, new Vector2(300, 600));
        player.localPlayerData.inventory.RemoveAll((x) => x is Weapon);
        player.localPlayerData.inventory.Add(new Weapon(player.weaponInTrigger.name));
        Destroy(player.weaponInTrigger);

        foreach (GameObject swapButton in GameObject.FindGameObjectsWithTag("swapButton"))
        {
            Destroy(swapButton);
        }
    }

    private void raycastTest()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(cam.WorldToScreenPoint(this.gameObject.transform.position)), out hitInfo);
        if (hit)
        {
            if (!hitInfo.transform.gameObject.Equals(this.gameObject) && hitInfo.transform.gameObject.CompareTag("stage"))
            {
                var color = hitInfo.transform.gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;
                color.a = 0.3f;
                hitInfo.transform.gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                hitInfo.transform.gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetInt("_ZWrite", 0);
                hitInfo.transform.gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                hitInfo.transform.gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.renderQueue = 3000;
                hitInfo.transform.gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = color;
            }
            else
            {
                foreach (GameObject elem in stageElems)
                {
                    var color = elem.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;
                    color.a = 1;
                    elem.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    elem.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetInt("_ZWrite", 1);
                    elem.GetComponentInChildren<MeshRenderer>().sharedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    elem.GetComponentInChildren<MeshRenderer>().sharedMaterial.renderQueue = -1;
                    elem.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = color;
                }
            }
        }
    }
}
