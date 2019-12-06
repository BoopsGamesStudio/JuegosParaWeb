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
        //Application.OpenURL("https://twitter.com/Boops_Games");
        Application.ExternalEval("window.open(\"https://twitter.com/Boops_Games\",\"_blank\")");
    }

    public void YoutubeBtn()
    {
        //Application.OpenURL("https://www.youtube.com/channel/UCdlggk1-f6dqdhcsiB29jWA");
        Application.ExternalEval("window.open(\"https://www.youtube.com/channel/UCdlggk1-f6dqdhcsiB29jWA\",\"_blank\")");
    }

    public void InstagramBtn()
    {
        //Application.OpenURL("https://www.instagram.com/boopsgamesstudio/");
        Application.ExternalEval("window.open(\"https://www.instagram.com/boopsgamesstudio\",\"_blank\")");
    }

    public void ItchioBtn()
    {
        //Application.OpenURL("https://itch.io/profile/boops-games-studio");
        Application.ExternalEval("window.open(\"https://itch.io/profile/boops-games-studio\",\"_blank\")");
    }

    public void BoopsBtn()
    {
        //Application.OpenURL("https://boopsgamesstudio.github.io/portfolio/");
        Application.ExternalEval("window.open(\"https://boopsgamesstudio.github.io/portfolio\",\"_blank\")");
    }

    public void BackBtn()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
