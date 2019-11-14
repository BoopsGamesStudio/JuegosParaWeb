using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Tooltip("Only applies at Scene1")]
    [SerializeField] float timeLeft;

    [Tooltip("Only applies at MenuScene")]
    [SerializeField] Button jugar;

    List<Item> sceneObjs;

    #region Generate properties
    enum objType{buff, weapon, consumable};
     enum objLvl { S, M, L };

    List<float> SpawnPos;
    List<string> ItemsS;
    List<string> ItemsM;
    List<string> ItemsL;

    int objsInSceneS = 1;
    int objsInSceneM = 1;
    int objsInSceneL = 1;
    #endregion

    private void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject stage = GameObject.FindGameObjectWithTag("stage");
        GameObject center = GameObject.FindGameObjectWithTag("pivot");
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");

        foreach (GameObject player in players)
        {
            DontDestroyOnLoad(player);
        }
        DontDestroyOnLoad(manager);
        DontDestroyOnLoad(stage);
        DontDestroyOnLoad(center);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            Button b = jugar.GetComponent<Button>();
            b.onClick.AddListener(loadScene);
        }
        if (SceneManager.GetActiveScene().name == "Scene1")
        {
            sceneObjs = new List<Item>();
            initGenerateProperties();
            generateObjs();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Scene1") {
            timeLeft -= Time.deltaTime;

            GetComponentInChildren<Text>().text = "Time left: " + timeLeft.ToString("F1");

            if (timeLeft < 0)
            {
                SceneManager.LoadScene("Scene2");
            }
        }
    }

    void loadScene()
    {
        SceneManager.LoadScene("Scene1");
    }

    void initGenerateProperties()
    {
        SpawnPos = new List<float>{-5.31f,2.8f,1.53f,-1.15f,2.8f,1.53f,-0.97f,2.8f,6.1f};
        ItemsS = new List<string>{"SbuffS", "buff", "SbuffI", "buff", "SbuffE", "buff", "sword", "weapon", "bomb", "consumable"};
        ItemsM = new List<string> { "MbuffS", "buff", "MbuffI", "buff", "MbuffE", "buff", "shield", "weapon","trap", "consumable"};
        ItemsL = new List<string> { "LbuffS", "buff", "LbuffI", "buff", "LbuffE", "buff", "cannon", "weapon", "star", "consumable"};

    }

    void generateObjs()
    {
        for (int i = 0; i < objsInSceneS; i++)
        {
            var itemId = Random.Range(0, ItemsS.Count / 2 - 1) * 2;
            var posId = Random.Range(0, SpawnPos.Count / 3 - 1) * 3;
            createObjS(itemId, posId);
        }

        for (int i = 0; i < objsInSceneM; i++)
        {
            var itemId = Random.Range(0, ItemsM.Count / 2 - 1) * 2;
            var posId = Random.Range(0, SpawnPos.Count / 3 - 1) * 3;
            createObjM(itemId, posId);
        }

        for (int i = 0; i < objsInSceneL; i++)
        {
            var itemId = Random.Range(0, ItemsL.Count / 2 - 1) * 2;
            var posId = Random.Range(0, SpawnPos.Count / 3 - 1) * 3;
            createObjL(itemId, posId);
        }
    } 
    
    void createObjS(int itemId, int posId)
    {
        Debug.Log("He entrado S");
        GameObject model;
        var type = ItemsS[itemId + 1];
        model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        model.transform.position = new Vector3(SpawnPos[posId], SpawnPos[posId + 1], SpawnPos[posId + 2]);
        model.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        model.name = ItemsS[itemId];
        model.tag = type;
        model.AddComponent<SphereCollider>();
        model.GetComponent<SphereCollider>().radius = 0.5f;
        model.GetComponent<SphereCollider>().isTrigger = true;
        objsInSceneS--;
        SpawnPos.RemoveRange(posId, 3);
    }

    void createObjM(int itemId, int posId)
    {
        Debug.Log("He entrado M");
        GameObject model;
        var type = ItemsM[itemId + 1];
        model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        model.transform.position = new Vector3(SpawnPos[posId], SpawnPos[posId + 1], SpawnPos[posId + 2]);
        model.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        model.name = ItemsM[itemId];
        model.tag = type;
        model.AddComponent<SphereCollider>();
        model.GetComponent<SphereCollider>().radius = 0.5f;
        model.GetComponent<SphereCollider>().isTrigger = true;
        objsInSceneM--;
        SpawnPos.RemoveRange(posId, 3);

    }

    void createObjL(int itemId, int posId)
    {
        Debug.Log("He entrado L");
        GameObject model;
        var type = ItemsL[itemId + 1];
        model = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        model.transform.position = new Vector3(SpawnPos[posId], SpawnPos[posId + 1], SpawnPos[posId + 2]);
        model.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        model.name = ItemsL[itemId];
        model.tag = type;
        model.AddComponent<SphereCollider>();
        model.GetComponent<SphereCollider>().radius = 0.5f;
        model.GetComponent<SphereCollider>().isTrigger = true;
        objsInSceneL--;
        SpawnPos.RemoveRange(posId, 3);

    }
}

