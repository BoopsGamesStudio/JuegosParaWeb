using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class How2PlayController : MonoBehaviour
{
    private int page;
    [SerializeField]
    private Button nextBtn;
    [SerializeField]
    private Button prevBtn;

    private void Awake()
    {
        if (Application.isMobilePlatform)
            FindObjectOfType<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    public void Start()
    {
        page = 1;
    }

    private void updatePage()
    {
        switch (page)
        {
            case 1:
                prevBtn.interactable = false; 
                break;

            case 2:
                prevBtn.interactable = true;
                break;

            case 3:
                nextBtn.interactable = true;
                break;

            case 4:
                nextBtn.interactable = false;
                break;
        }
    }

    public void NextBtn()
    {
        page++;
        updatePage();
    }
    public void PrevBtn()
    {
        page--;
        updatePage();
    }
    public void backBtn()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
