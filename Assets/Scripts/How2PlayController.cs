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
    [SerializeField]
    private GameObject page1_pc;
    [SerializeField]
    private GameObject page1_mobile;
    [SerializeField]
    private GameObject page2;
    [SerializeField]
    private GameObject page3;
    [SerializeField]
    private GameObject page4;
    [SerializeField]
    private GameObject page5;
    [SerializeField]
    private GameObject page6;

    private void Awake()
    {
        if (Application.isMobilePlatform)
            FindObjectOfType<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    public void Start()
    {
        page = 1;
        if (Application.isMobilePlatform)
            page1_mobile.SetActive(true);
        else
            page1_pc.SetActive(true);
    }

    private void updatePage()
    {
        switch (page)
        {
            case 1:
                prevBtn.interactable = false;
                page2.SetActive(false);
                if (Application.isMobilePlatform)
                    page1_mobile.SetActive(true);
                else
                    page1_pc.SetActive(true);
                break;

            case 2:
                prevBtn.interactable = true;
                if (Application.isMobilePlatform)
                    page1_mobile.SetActive(false);
                else
                    page1_pc.SetActive(false);
                page3.SetActive(false);
                page2.SetActive(true);
                break;

            case 3:
                page2.SetActive(false);
                page4.SetActive(false);
                page3.SetActive(true);
                break;

            case 4:
                page3.SetActive(false);
                page5.SetActive(false);
                page4.SetActive(true);
                break;

            case 5:
                nextBtn.interactable = true;
                page4.SetActive(false);
                page6.SetActive(false);
                page5.SetActive(true);
                break;

            case 6:
                nextBtn.interactable = false;
                page5.SetActive(false);
                page6.SetActive(true);
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
