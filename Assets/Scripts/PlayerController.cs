using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
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
        Debug.Log("Start");
        localPlayerData = new PlayerStatistics();
        //GlobalControl.Instance.savedPlayerData = new List<PlayerStatistics>();
        initPlayerStats();

        joystick = FindObjectOfType<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Player")) {
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
                Vector3 vel = new Vector3(-joystick.Horizontal * localPlayerData.movementSpeed, gameObject.GetComponent<Rigidbody>().velocity.y, -joystick.Vertical * localPlayerData.movementSpeed);

                vel = Quaternion.AngleAxis(cameraAnlgeOffset, Vector3.up) * vel;

                gameObject.GetComponent<Rigidbody>().velocity = vel;
                if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
                {
                    Vector3 dir = gameObject.GetComponent<Rigidbody>().velocity.normalized;
                    dir.y = 0;
                    this.transform.rotation = Quaternion.LookRotation(dir);
                }
            } else
            {
                x = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed;
                z = Input.GetAxis("Vertical") * Time.deltaTime * localPlayerData.movementSpeed;

                transform.Rotate(0, x, 0);
                transform.Translate(0, 0, z);
            }
        }

        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                foreach (GameObject o in GameObject.FindGameObjectsWithTag("Dummy")) {
                    if (gameObject.GetComponentInChildren<BoxCollider>().bounds.Contains(o.transform.position + Vector3.up) && !o.Equals(gameObject))
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
            foreach (Item i in localPlayerData.inventory) {
                Debug.Log(i.getAttribs());
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
            Debug.Log(col.gameObject.name);
            Destroy(col.gameObject);
            localPlayerData.inventory.Add(new Weapon(col.gameObject.name));

        }
        if (col.gameObject.CompareTag("consumable"))
        {
            Debug.Log(col.gameObject.name);
            Destroy(col.gameObject);
            localPlayerData.inventory.Add(new ConsumableItem(col.gameObject.name));

        }
        if (col.gameObject.CompareTag("CornerTrigger"))
        {
            switch (col.gameObject.name)
            {
                case "TriggerWest":
                    //previousCorner = currentCorner;
                    currentCorner = cornerNames.West;
                    break;
                case "TriggerSouth":
                    //previousCorner = currentCorner;
                    currentCorner = cornerNames.South;
                    break;
                case "TriggerNorth":
                    //previousCorner = currentCorner;
                    currentCorner = cornerNames.North;
                    break;
                case "TriggerEast":
                    //previousCorner = currentCorner;
                    currentCorner = cornerNames.East;
                    break;
            }
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
}
