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

        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            if (Application.isMobilePlatform)
            {
                camera.transform.Rotate(new Vector3(0, 0, 90));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null) this.transform.localPosition = new Vector3(this.transform.localPosition.x, player.transform.position.y + 15, this.transform.localPosition.z);
    }

    void Rescale(float targetaspect)
    {
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
