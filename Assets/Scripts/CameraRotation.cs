using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    float turningTime;
    [SerializeField] float speed;
    Vector3 currentAngle;
    Vector3 targetRot;
    Vector3 targetPos;
    GameObject player;
    PlayerController.cornerNames cameraCorner = PlayerController.cornerNames.South;
    PlayerController.sides cameraSide = PlayerController.sides.Center;

    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<PlayerController>().gameObject;

        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            Debug.Log(FindObjectsOfType<PlayerController>().Length);
            if (pc.GetComponent<PhotonView>().IsMine)
            {
                player = pc.gameObject;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(FindObjectsOfType<PlayerController>().Length);
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

        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, targetRot.y, turningTime), 0);
        this.transform.eulerAngles = currentAngle;

        this.transform.position = new Vector3(this.transform.position.x, player.transform.position.y + 2, this.transform.position.z);
        HorizontalPan();
    }

    void rotateTo(float angle)
    {
        Debug.Log(angle);
        targetRot = targetRot + new Vector3(0, angle, 0);
        turningTime = Time.deltaTime * speed;
    }

    void HorizontalPan()
    {
        switch (player.GetComponent<PlayerController>().currentSide)
        {
            case PlayerController.sides.Left:
                targetPos = new Vector3(8, this.transform.position.y, 4);
                cameraSide = PlayerController.sides.Left;
                break;
            case PlayerController.sides.Center:
                targetPos = new Vector3(-3, this.transform.position.y, 4);
                cameraSide = PlayerController.sides.Center;
                break;
            case PlayerController.sides.Right:
                targetPos = new Vector3(-14, this.transform.position.y, 4);
                cameraSide = PlayerController.sides.Right;
                break;
        }

        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * speed);
    }
}
