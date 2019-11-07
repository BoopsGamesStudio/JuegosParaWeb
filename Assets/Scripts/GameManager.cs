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

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MenuScene")
        {
            Button b = jugar.GetComponent<Button>();
            b.onClick.AddListener(loadScene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Scene1") {
            timeLeft -= Time.deltaTime;

            this.GetComponentInChildren<Text>().text = "Time left: " + timeLeft.ToString("F1");

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
}