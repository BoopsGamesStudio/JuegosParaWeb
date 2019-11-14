using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum cornerNames { North, East, South, West }

    [SerializeField] List<Item> inventory;
    [SerializeField] GameObject center;
 
    [SerializeField] float rotSpeed;
    [HideInInspector] public float x;
    [HideInInspector] public float z;
    //[HideInInspector] public cornerNames previousCorner;
    [HideInInspector] public cornerNames currentCorner = cornerNames.South;

    #region Stats
    private float impact = 2;
    private float movementSpeed = 2;
    private float endurance = 2;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        if (SceneManager.GetActiveScene().name == "Scene1")
        inventory = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed;
        z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Item i in inventory) {
                Debug.Log(i.getAttribs());
            }
            Debug.Log(impact);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("buff"))
        {
            Debug.Log(col.gameObject.name);
            Destroy(col.gameObject);
            inventory.Add(new BuffItem(col.gameObject.name));
             
        }
        if (col.gameObject.CompareTag("weapon"))
        {
            Debug.Log(col.gameObject.name);
            Destroy(col.gameObject);
            inventory.Add(new Weapon(col.gameObject.name));

        }
        if (col.gameObject.CompareTag("consumable"))
        {
            Debug.Log(col.gameObject.name);
            Destroy(col.gameObject);
            inventory.Add(new ConsumableItem(col.gameObject.name));

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
}
