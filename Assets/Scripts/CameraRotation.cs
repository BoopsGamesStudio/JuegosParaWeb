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
    PlayerController.cornerNames cameraCorner = PlayerController.cornerNames.South;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Rigidbody>().velocity.magnitude >= 0.1f)
        {
            switch (player.GetComponent<PlayerController>().currentCorner)
            {
                case PlayerController.cornerNames.North:
                    if (cameraCorner != PlayerController.cornerNames.North)
                    {
                        Debug.Log("N");
                        rotateTo((PlayerController.cornerNames.North - cameraCorner) * 90);
                        cameraCorner = PlayerController.cornerNames.North;
                    }
                    break;
                case PlayerController.cornerNames.East:
                    if (cameraCorner != PlayerController.cornerNames.East)
                    {
                        Debug.Log("E");
                        rotateTo((PlayerController.cornerNames.East - cameraCorner) * 90);
                        cameraCorner = PlayerController.cornerNames.East;
                    }
                    break;
                case PlayerController.cornerNames.South:
                    if (cameraCorner != PlayerController.cornerNames.South)
                    {
                        Debug.Log("S");
                        rotateTo((PlayerController.cornerNames.South - cameraCorner) * 90);
                        cameraCorner = PlayerController.cornerNames.South;
                    }
                    break;
                case PlayerController.cornerNames.West:
                    if (cameraCorner != PlayerController.cornerNames.West)
                    {
                        Debug.Log("W");
                        rotateTo((PlayerController.cornerNames.West - cameraCorner) * 90);
                        cameraCorner = PlayerController.cornerNames.West;
                    }
                    break;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                rotateTo(90);
                if (cameraCorner == PlayerController.cornerNames.West)
                {
                    cameraCorner = PlayerController.cornerNames.North;
                }
                else
                {
                    cameraCorner++;
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                rotateTo(-90);
                if (cameraCorner == PlayerController.cornerNames.North)
                {
                    cameraCorner = PlayerController.cornerNames.West;
                }
                else
                {
                    cameraCorner--;
                }
            }
        }

        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, targetRot.y, turningTime), 0);
        this.transform.eulerAngles = currentAngle;
    }

    void rotateTo(float angle)
    {
        Debug.Log(angle);
        targetRot = targetRot + new Vector3(0, angle, 0);
        turningTime = Time.deltaTime * speed;
    }
}
