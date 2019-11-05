using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        this.GetComponentInChildren<UnityEngine.UI.Text>().text = "Time left: " + timeLeft.ToString("F1");

        if (timeLeft < 0)
        {
            SceneManager.LoadScene("Scene2");
        }
    }
}
