using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource menuSound;

    public void MenuSound()
    {
        menuSound.Play();
    }
}
