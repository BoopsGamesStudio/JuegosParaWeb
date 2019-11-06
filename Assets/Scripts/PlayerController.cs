using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum cornerNames { North, East, South, West }

    [SerializeField] List<Item> inventory;
    [SerializeField] GameObject center;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotSpeed;
    [HideInInspector] public float x;
    [HideInInspector] public float z;
    [HideInInspector] public cornerNames previousCorner;
    [HideInInspector] public cornerNames currentCorner;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {
         x = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed;
         z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(inventory.Count);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Item"))
        {
            Item newItem = new Item(col.gameObject.name);
            Destroy(col.gameObject);

            inventory.Add(newItem);
        } else {
            switch (col.gameObject.name)
            {
                case "TriggerWest":
                    previousCorner = currentCorner;
                    currentCorner = cornerNames.West;
                    break;
                case "TriggerSouth":
                    previousCorner = currentCorner;
                    currentCorner = cornerNames.South;
                    break;
                case "TriggerNorth":
                    previousCorner = currentCorner;
                    currentCorner = cornerNames.North;
                    break;
                case "TriggerEast":
                    previousCorner = currentCorner;
                    currentCorner = cornerNames.East;
                    break;
            }
        }
    }
}
