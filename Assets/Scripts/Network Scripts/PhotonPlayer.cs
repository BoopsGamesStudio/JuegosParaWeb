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
    bool canCreate;

    void Start()
    {
        robot = GlobalControl.Instance.savedPlayerData.model;
        //if (PhotonNetwork.IsMasterClient) canCreate = true;
        CreatePlayer();
        if (SceneManager.GetActiveScene().name == "Scene1" || SceneManager.GetActiveScene().name == "Level2" || SceneManager.GetActiveScene().name == "Level3")
        {
            Instantiate(prefabCenter);
        }
    }

    private void Update()
    {
        /*if(canCreate)
        {
            CreatePlayer();
        }*/
    }

    private void CreatePlayer()
    {
        //canCreate = false;
        int spawnPick = Random.Range(0, GameSetup.GS.spawPoints.Count);
        GetComponent<PhotonView>().RPC("RPC_RemoveSpawnPoint", RpcTarget.Others, spawnPick);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", robot), GameSetup.GS.spawPoints[spawnPick].position,
          GameSetup.GS.spawPoints[spawnPick].rotation, 0);

        //GetComponent<PhotonView>().RPC("RPC_CreatePlayer", RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        canCreate = true;
    }

    [PunRPC]
    private void RPC_RemoveSpawnPoint(int spawnPick)
    {
        GameSetup.GS.spawPoints.RemoveAt(spawnPick);
    }
}
