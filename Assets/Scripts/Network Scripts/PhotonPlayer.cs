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
        if (SceneManager.GetActiveScene().name == "SearchLevel")
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
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", robot), GameSetup.GS.spawPoints[spawnPick].position,
          GameSetup.GS.spawPoints[spawnPick].rotation, 0);

        if(SceneManager.GetActiveScene().name == "BattleScene1" || SceneManager.GetActiveScene().name == "BattleScene2" || SceneManager.GetActiveScene().name == "BattleScene3")
        {
            if (GlobalControl.Instance.savedPlayerData.inventory.Exists((x) => x is Weapon))
            {
                Weapon weapon = GlobalControl.Instance.savedPlayerData.getWeapon();

                string weaponName = weapon.getName();
                Debug.Log(weaponName + " ha pasado");
                Transform weaponHand = FindObject(player, weaponName).transform;
                player.GetComponent<PhotonView>().RPC("RPC_LoadWeapon", RpcTarget.All, player.GetComponent<PhotonView>().Owner.ActorNumber, weaponHand.GetSiblingIndex());
            }
        }
    }

    private GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }

        return null;
    }
}
