using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    [SerializeField]
    private Text timerToStartDisplay;
    [SerializeField]
    private float timerToStartGame;
    [SerializeField]
    private GameObject PhoneInterface;

    private bool sceneLoaded;
    private int battleSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isMobilePlatform)
        {
            PhoneInterface.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timerToStartGame -= Time.deltaTime;
        timerToStartDisplay.text = timerToStartGame.ToString("F1");
        if (timerToStartGame <= 0)
        {
            if (PhotonNetwork.IsMasterClient && !sceneLoaded)
            {
                sceneLoaded = true;
                battleSceneIndex = Random.Range(6, 9);
                PhotonNetwork.LoadLevel(battleSceneIndex);
            }
        }
    }
}
