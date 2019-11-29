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
    public GameObject player;
    PlayerController.cornerNames cameraCorner = PlayerController.cornerNames.South;
    PlayerController.sides cameraSide = PlayerController.sides.Center;

    // Start is called before the first frame update
    void Start()
    {
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            if (pc.PV.IsMine)
            {
                player = pc.gameObject;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
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

        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, targetRot.y, turningTime), 0);
        this.transform.eulerAngles = currentAngle;

        if (player != null)
        {
            Debug.Log("Player not null");
            //if(player.transform.position.y + 2 != this.transform.localPosition.y)
            //this.transform.position = Vector3.Lerp(this.transform.position,new Vector3(this.transform.position.x, player.transform.position.y + 2, this.transform.position.z), Time.deltaTime * speed);
            this.transform.position = new Vector3(this.transform.position.x, player.transform.position.y + 2, this.transform.position.z);
            if (cameraSide != player.GetComponent<PlayerController>().currentSide)
                HorizontalPan();
        }
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
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(8, 0, 4), Time.deltaTime * speed);
                break;
            case PlayerController.sides.Center:
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(-3, 0, 4), Time.deltaTime * speed);
                break;
            case PlayerController.sides.Right:
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(-14, 0, 4), Time.deltaTime * speed);
                break;
        }
    }
}
