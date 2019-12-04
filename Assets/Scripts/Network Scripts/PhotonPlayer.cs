using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonPlayer : MonoBehaviour
{
    [SerializeField] GameObject prefabCenter;
    string robot;

    void Start()
    {
        robot = GlobalControl.Instance.savedPlayerData.model;
        CreatePlayer();
        if (SceneManager.GetActiveScene().name == "Scene1" || SceneManager.GetActiveScene().name == "Level2" || SceneManager.GetActiveScene().name == "Level3")
        {
            Instantiate(prefabCenter);
        }
    }

    private void Update()
    {

    }

    private void CreatePlayer()
    {
        int spawnPick = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", robot), GameSetup.GS.spawPoints[spawnPick].position,
          GameSetup.GS.spawPoints[spawnPick].rotation, 0);
    }
}
