using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    float turningTime;
    [SerializeField] float speed;
    Vector3 currentAngle;
    Vector3 currentCamAngle;
    Vector3 targetRot;
    bool tiltingDown = false;

    float camTurningTime;
    Vector3 targetCamRot = new Vector3(25, 135, 0);
    float targetPos = 15;
    float tiltRotation = 20;

    GameObject player;
    GameObject cam;
    PlayerController.cornerNames cameraCorner;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (p.GetPhotonView().IsMine)
            {
                cameraCorner = p.GetComponent<PlayerController>().currentCorner;
            }
        }

        switch (cameraCorner)
        {
            case PlayerController.cornerNames.West:
                targetRot = targetRot + new Vector3(0, 90, 0);
                currentAngle = new Vector3(this.transform.eulerAngles.x, 90, this.transform.eulerAngles.z);
                this.transform.eulerAngles = currentAngle;
                break;
            case PlayerController.cornerNames.North:
                targetRot = targetRot + new Vector3(0, 180, 0);
                currentAngle = new Vector3(this.transform.eulerAngles.x, 180, this.transform.eulerAngles.z);
                this.transform.eulerAngles = currentAngle;
                break;
            case PlayerController.cornerNames.East:
                targetRot = targetRot + new Vector3(0, -90, 0);
                currentAngle = new Vector3(this.transform.eulerAngles.x, -90, this.transform.eulerAngles.z);
                this.transform.eulerAngles = currentAngle;
                break;
            default:
                break;
        }

        cam = FindObjectOfType<Camera>().gameObject;

        currentCamAngle = cam.transform.eulerAngles;

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

        if (player.GetComponent<PlayerController>().cameraHorizontal)
        {
            if (!tiltingDown)
            {
                tiltingDown = true;
                rotateCamTo(-tiltRotation);
                targetPos = 2;
            }

        } else
        {
            if (tiltingDown)
            {
                tiltingDown = false;
                rotateCamTo(tiltRotation);
                targetPos = 15;
            }

        }

        //girar y bajar camara
        currentCamAngle = new Vector3(Mathf.LerpAngle(currentCamAngle.x, targetCamRot.x, camTurningTime), cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
        cam.transform.eulerAngles = currentCamAngle;
        cam.transform.position = new Vector3(cam.transform.position.x, Mathf.Lerp(cam.transform.position.y, targetPos, camTurningTime), cam.transform.position.z);

        //girar center
        currentAngle = new Vector3(this.transform.eulerAngles.x, Mathf.LerpAngle(currentAngle.y, targetRot.y, turningTime), this.transform.eulerAngles.z);
        this.transform.eulerAngles = currentAngle;

        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (p.GetPhotonView().IsMine)
            {
                p.GetComponent<PlayerController>().currentCorner = cameraCorner;
            }

        }
    }

    void rotateTo(float angle)
    {
        Debug.Log(angle);
        targetRot = targetRot + new Vector3(0, angle, 0);
        turningTime = Time.deltaTime * speed;
    }

    void rotateCamTo(float angle)
    {
        Debug.Log(angle);
        targetCamRot = targetCamRot + new Vector3(angle, 0, 0);
        camTurningTime = Time.deltaTime * speed;
    }
}
