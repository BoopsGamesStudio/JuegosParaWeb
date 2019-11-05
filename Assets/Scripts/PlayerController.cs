using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum cornerNames { North, East, South, West }

    [SerializeField] GameObject center;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotSpeed;
    [HideInInspector] public cornerNames previousCorner;
    [HideInInspector] public cornerNames currentCorner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }

    private void OnTriggerEnter(Collider col)
    {
        switch(col.gameObject.name)
        {
            case "Trigger 1,1":
                previousCorner = currentCorner;
                currentCorner = cornerNames.West;
                break;
            case "Trigger -1,1":
                previousCorner = currentCorner;
                currentCorner = cornerNames.South;
                break;
            case "Trigger 1,-1":
                previousCorner = currentCorner;
                currentCorner = cornerNames.North;
                break;
            case "Trigger -1,-1":
                previousCorner = currentCorner;
                currentCorner = cornerNames.East;
                break;
        }
    }
}
