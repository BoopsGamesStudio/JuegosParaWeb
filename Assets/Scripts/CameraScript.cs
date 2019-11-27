using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    Camera camera;
    bool alreadyTilted = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<Camera>();
        Rescale();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null) this.transform.localPosition = new Vector3(this.transform.localPosition.x, player.transform.position.y + 15, this.transform.localPosition.z);

        Rescale();
    }

    void Rescale()
    {
        float targetaspect = 9.0f / 16.0f;

        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            Debug.Log(Screen.width);
            if (Screen.width < Screen.height)
            {
                foreach (GameObject existingWarn in GameObject.FindGameObjectsWithTag("RotateWarn"))
                {
                    Destroy(existingWarn);
                }

                GameObject warn = new GameObject();
                warn.tag = "RotateWarn";

                Text text = warn.AddComponent<Text>();
                text.text = "ROTALO, BITCH";
                text.font = Resources.Load<Font>("ARIAL");
                text.fontSize = 100;
                text.alignment = TextAnchor.MiddleCenter;

                warn.transform.SetParent(FindObjectOfType<Canvas>().transform);
                warn.GetComponent<RectTransform>().localScale = Vector3.one;
                warn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                warn.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600);
                warn.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 600);
            }
            else
            {
                foreach (GameObject warn in GameObject.FindGameObjectsWithTag("RotateWarn"))
                {
                    Destroy(warn);
                }

                Screen.orientation = ScreenOrientation.LandscapeLeft;
                targetaspect = 16.0f / 9.0f;

                if (!alreadyTilted)
                {
                    camera.transform.Rotate(new Vector3(0, 0, -90));
                    alreadyTilted = true;
                }
            }
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
            targetaspect = 9.0f / 16.0f;
        }

        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
