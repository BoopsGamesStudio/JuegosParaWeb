﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lean.Localization;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private int menuSceneIndex;

    [SerializeField]
    private Text soundText;

    private void Awake()
    {
        if (Application.isMobilePlatform)
            FindObjectOfType<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    private void Start()
    {
        switch (AudioListener.volume)
        {
            case 0:
                soundText.text = "OFF";
                soundText.color = Color.red;
                break;
            case 1:
                soundText.text = "ON";
                soundText.color = Color.green;
                break;
        }
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

    public void SoundBtn()
    {
        switch (AudioListener.volume)
        {
            case 1:
                AudioListener.volume = 0;
                soundText.text = "OFF";
                soundText.color = Color.red;
                break;
            case 0:
                AudioListener.volume = 1;
                soundText.text = "ON";
                soundText.color = Color.green;
                break;
        }
    }
}
