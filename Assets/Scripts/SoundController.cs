using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource sound;

    public void BtnPress()
    {
        sound.Play();
    }
}
