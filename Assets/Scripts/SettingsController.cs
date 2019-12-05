using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lean.Localization;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private int menuSceneIndex;

    public void SpanishBtn()
    {
        LeanLocalization.CurrentLanguage = "Spanish";
    }

    public void EnglishBtn()
    {
        LeanLocalization.CurrentLanguage = "English";
    }

    public void BackBtn()
    {
        SceneManager.LoadScene(menuSceneIndex);
    }
}
