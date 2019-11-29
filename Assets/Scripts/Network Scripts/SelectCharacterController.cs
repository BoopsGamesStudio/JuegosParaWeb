using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SelectCharacterController : MonoBehaviour
{
    [SerializeField] Transform displayPoint;
    [SerializeField]
    private GameObject rotLButton;
    [SerializeField]
    private GameObject rotRButton;
    [SerializeField]
    private GameObject StartButton;
    float turningTime;
    [SerializeField] float speed;
    Vector3 currentAngle;
    Vector3 targetRot;
    List<string> robots;
    string robot;

    // Start is called before the first frame update
    void Start()
    {
        robots = new List<string> { "cabeza_equilibrio", "cabeza_ataque", "cabeza_defensa", "cabeza_velocidad"};
        robot = "cabeza_equilibrio";
        DisplayCharacters();
    }

    private void Update()
    {
        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, targetRot.y, turningTime), 0);
        this.transform.eulerAngles = currentAngle;
        Debug.Log(robot);
    }

    void rotateTo(float angle)
    {
        targetRot = targetRot + new Vector3(0, angle, 0);
        turningTime = Time.deltaTime * speed;
    }

    private void DisplayCharacters()
    {
        var i = 0;
        foreach (string robot in robots)
        {
            Vector3 baseDir = new Vector3(0, 0, -1);
            Vector3 localDir = Quaternion.Euler(0, i * 360 / robots.Count,0) * baseDir;

            Vector3 offset = localDir - displayPoint.position;

            Vector3 displayPos = displayPoint.position + localDir.normalized * robots.Count * 1.0f;
            GameObject model = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", robot), displayPos, Quaternion.LookRotation(localDir),0);
            model.transform.SetParent(this.transform);
            i++;
        }

    }

    public void rotLBtn()
    {
        rotateTo(-360 / robots.Count);
        var index = robots.IndexOf(robot);
        if (index != (robots.Count - 1))
        {
            robot = robots[index + 1];
        }
        else
        {
            robot = robots[(robots.Count - 1) - index];
        }
    }

    public void rotRBtn()
    {
        rotateTo(360/robots.Count);
        var index = robots.IndexOf(robot);
        if (index != 0)
        {
            robot = robots[index - 1];
        }
        else
        {
            robot = robots[(robots.Count - 1) - index];
        }
    }

    public void StartBtn()
    {

    }
}
