using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContactController : MonoBehaviour
{

    private void Awake()
    {
        if (Application.isMobilePlatform)
            FindObjectOfType<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    public void TwitterBtn()
    {
        Application.OpenURL("https://twitter.com/Boops_Games");
    }

    public void YoutubeBtn()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCdlggk1-f6dqdhcsiB29jWA");
    }

    public void InstagramBtn()
    {
        Application.OpenURL("https://www.instagram.com/boopsgamesstudio/");
    }

    public void ItchioBtn()
    {
        Application.OpenURL("https://itch.io/profile/boops-games-studio");
    }

    public void BoopsBtn()
    {
        Application.OpenURL("https://boopsgamesstudio.github.io/portfolio/");
    }

    public void BackBtn()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
