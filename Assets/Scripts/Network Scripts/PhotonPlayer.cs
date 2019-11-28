using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        int spawnPick = Random.Range(0, GameSetup.GS.spawPoints.Length);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonRoboto"), GameSetup.GS.spawPoints[spawnPick].position,
          GameSetup.GS.spawPoints[spawnPick].rotation, 0);
    }

}
