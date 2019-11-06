using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    float turningTime;
    [SerializeField] float speed;
    Vector3 currentAngle;
    Vector3 targetRot;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (player.GetComponent<PlayerController>().currentCorner)
        {
            case PlayerController.cornerNames.West:
                if (player.GetComponent<PlayerController>().previousCorner == PlayerController.cornerNames.South) {
                    rotateTo(90);
                    player.GetComponent<PlayerController>().previousCorner = PlayerController.cornerNames.West;
                } else if (player.GetComponent<PlayerController>().previousCorner == PlayerController.cornerNames.North)
                {
                    rotateTo(-90);
                    player.GetComponent<PlayerController>().previousCorner = PlayerController.cornerNames.West;
                }
                break;
            case PlayerController.cornerNames.South:
                if (player.GetComponent<PlayerController>().previousCorner == PlayerController.cornerNames.East)
                {
                    rotateTo(90);
                    player.GetComponent<PlayerController>().previousCorner = PlayerController.cornerNames.South;
                }
                else if (player.GetComponent<PlayerController>().previousCorner == PlayerController.cornerNames.West)
                {
                    rotateTo(-90);
                    player.GetComponent<PlayerController>().previousCorner = PlayerController.cornerNames.South;
                }
                break;
            case PlayerController.cornerNames.North:
                if (player.GetComponent<PlayerController>().previousCorner == PlayerController.cornerNames.West)
                {
                    rotateTo(90);
                    player.GetComponent<PlayerController>().previousCorner = PlayerController.cornerNames.North;
                }
                else if (player.GetComponent<PlayerController>().previousCorner == PlayerController.cornerNames.East)
                {
                    rotateTo(-90);
                    player.GetComponent<PlayerController>().previousCorner = PlayerController.cornerNames.North;
                }
                break;
            case PlayerController.cornerNames.East:
                if (player.GetComponent<PlayerController>().previousCorner == PlayerController.cornerNames.North)
                {
                    rotateTo(90);
                    player.GetComponent<PlayerController>().previousCorner = PlayerController.cornerNames.East;
                }
                else if (player.GetComponent<PlayerController>().previousCorner == PlayerController.cornerNames.South)
                {
                    rotateTo(-90);
                    player.GetComponent<PlayerController>().previousCorner = PlayerController.cornerNames.East;
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.O) && player.GetComponent<PlayerController>().x == 0 && player.GetComponent<PlayerController>().z == 0)
        {
            rotateTo(90);
        }

        if (Input.GetKeyDown(KeyCode.P) && player.GetComponent<PlayerController>().x == 0 && player.GetComponent<PlayerController>().z == 0)
        {
            rotateTo(-90);
        }

        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, targetRot.y, turningTime), 0);
        this.transform.eulerAngles = currentAngle;
    }

    void rotateTo(float angle)
    {
        //currentAngle = this.transform.eulerAngles;
        targetRot = targetRot + new Vector3(0, angle, 0);
        turningTime = Time.deltaTime * speed;
    }
}
