using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lean.Localization;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private int menuSceneIndex;
    Canvas mainCanvas;

    [SerializeField]
    private Text soundText;

    private void Start()
    {
        switch (AudioListener.volume)
        {
            case 0:
                soundText.color = Color.red;
                soundText.text = "OFF";
                break;
            case 1:
                soundText.color = Color.green;
                soundText.text = "ON";
                break;
        }
    }

    private void Awake()
    {
        foreach (Canvas canv in FindObjectsOfType<Canvas>())
        {
            if (canv.name == "AltCanvas")
                continue;

            mainCanvas = canv;
        }

        if (Application.isMobilePlatform)
            mainCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    private void Update()
    {
        switch (LeanLocalization.CurrentLanguage)
        {
            case "Spanish":
                foreach (Image other in FindObjectOfType<Canvas>().GetComponentsInChildren<Image>())
                {
                    if (other.gameObject.name == "SpainBtn" )
                        other.color = new Color(other.color.r, other.color.g, other.color.b, 1);
                    else if(other.gameObject.name != "BackBtn")
                        other.color = new Color(other.color.r, other.color.g, other.color.b, 0.3f);
                }
                break;
            case "English":
                foreach (Image other in FindObjectOfType<Canvas>().GetComponentsInChildren<Image>())
                {
                    if (other.gameObject.name == "EnglishBtn")
                        other.color = new Color(other.color.r, other.color.g, other.color.b, 1);
                    else if(other.gameObject.name != "BackBtn")
                        other.color = new Color(other.color.r, other.color.g, other.color.b, 0.3f);
                }
                break;
        }
    }

    public void SpanishBtn(Image button)
    {
        LeanLocalization.CurrentLanguage = "Spanish";
        
    }

    public void EnglishBtn(Image button)
    {
        LeanLocalization.CurrentLanguage = "English";
    }

    public void BackBtn()
    {
        SceneManager.LoadScene(menuSceneIndex);
    }

    public void soundBtn()
    {
        switch (AudioListener.volume)
        {
            case 1:
                AudioListener.volume = 0;
                soundText.color = Color.red;
                soundText.text = "OFF";
                break;
            case 0:
                AudioListener.volume = 1;
                soundText.color = Color.green;
                soundText.text = "ON";
                break;
        }
    }
}
