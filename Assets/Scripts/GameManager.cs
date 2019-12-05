using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;
using Lean.Localization;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeLeft;
    [SerializeField] Button jugar;

    List<Item> sceneObjs;

    int battleSceneIndex;
    bool sceneLoaded;

    #region Generate properties
    enum objType { buff, weapon, consumable };
    enum objLvl { S, M, L };

    List<string> ItemsS;
    List<string> ItemsM;
    List<string> ItemsL;

    int objsInSceneS = 7;
    int objsInSceneM = 4;
    int objsInSceneL = 2;
    #endregion

    public List<Transform> spawnPoints;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            Button b = jugar.GetComponent<Button>();
            b.onClick.AddListener(() => SceneManager.LoadScene("LobbyScene"));
        }
        if (SceneManager.GetActiveScene().name == "SearchLevel")
        {
            sceneObjs = new List<Item>();
            initGenerateProperties();
            if (PhotonNetwork.IsMasterClient)
            {
                generateObjs();
            }

            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                player.GetComponentInChildren<BoxCollider>().enabled = false;
            }
        }
        if(SceneManager.GetActiveScene().name == "BattleScene1" || SceneManager.GetActiveScene().name == "BattleScene2" || SceneManager.GetActiveScene().name == "BattleScene3")
        {
            foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (p.GetPhotonView().IsMine)
                    p.GetComponent<PlayerController>().savePlayer();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "SearchLevel")
        {
            timeLeft -= Time.deltaTime;

            GetComponentInChildren<Text>().text = timeLeft.ToString("F1");

            if (timeLeft < 0)
            {
                if (PhotonNetwork.IsMasterClient && !sceneLoaded)
                {
                    sceneLoaded = true;
                    battleSceneIndex = Random.Range(5, 8);
                    PhotonNetwork.LoadLevel(battleSceneIndex);
                }
            }
        }
    }

    void initGenerateProperties()
    {
        ItemsS = new List<string>{"SbuffS", "buff", "SbuffI", "buff", "SbuffE", "buff", "Dagger", "weapon", "SbuffS", "buff", "SbuffI", "buff",
                                  "SbuffE", "buff", "Buckler", "weapon", "SbuffS", "buff", "SbuffI", "buff", "SbuffE", "buff", "Plasma Handgun","weapon" };

        ItemsM = new List<string> { "MbuffS", "buff", "MbuffI", "buff", "MbuffE", "buff", "Sword", "weapon","Axe", "weapon", "MbuffS", "buff",
                                    "MbuffI", "buff", "MbuffE", "buff", "Tear Shield", "weapon", "Medium Shield", "weapon", "MbuffS", "buff",
                                    "MbuffI", "buff", "MbuffE", "buff", "Plasma Submachine", "weapon", "Plasma Shotgun", "weapon", };

        ItemsL = new List<string> { "LbuffS", "buff", "LbuffI", "buff", "LbuffE", "buff", "Spear", "weapon", "Sharp Shield", "weapon", "Plasma Cannon", "weapon" };

    }

    void generateObjs()
    {
        for (int i = 0; i < objsInSceneS; i++)
        {
            var itemId = Random.Range(0, ItemsS.Count / 2 - 1) * 2;
            var posId = Random.Range(0, spawnPoints.Count - 1);
            createObjS(itemId, posId);
        }

        for (int i = 0; i < objsInSceneM; i++)
        {
            var itemId = Random.Range(0, ItemsM.Count / 2 - 1) * 2;
            var posId = Random.Range(0, spawnPoints.Count - 1);
            createObjM(itemId, posId);
        }

        for (int i = 0; i < objsInSceneL; i++)
        {
            var itemId = Random.Range(0, ItemsL.Count / 2 - 1) * 2;
            var posId = Random.Range(0, spawnPoints.Count - 1);
            createObjL(itemId, posId);
        }
    }

    void createObjS(int itemId, int posId)
    {
        var name = ItemsS[itemId];
        var type = ItemsS[itemId + 1];
        PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", name), spawnPoints[posId].position, spawnPoints[posId].rotation);
        objsInSceneS--;
        spawnPoints.RemoveAt(posId);
    }

    void createObjM(int itemId, int posId)
    {
        var name = ItemsM[itemId];
        var type = ItemsM[itemId + 1];
        PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", name), spawnPoints[posId].position, spawnPoints[posId].rotation);
        objsInSceneM--;
        spawnPoints.RemoveAt(posId);
    }

    void createObjL(int itemId, int posId)
    {
        var name = ItemsL[itemId];
        var type = ItemsL[itemId + 1];
        PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", name), spawnPoints[posId].position, spawnPoints[posId].rotation);
        objsInSceneL--;
        spawnPoints.RemoveAt(posId);
    }

    public void settingsBtn()
    {
        SceneManager.LoadScene(10);
    }
}

