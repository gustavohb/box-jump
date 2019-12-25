using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent (typeof(CanvasGroup))]
public class OptionsMenuController : MonoBehaviour
{
    public TextMeshProUGUI musicButton;
    public TextMeshProUGUI sfxButton;

    public AudioClip clickSound;

    public GameObject mainMenu;

    private void Start()
    {

        if (AudioManager.Instance != null)
        {
            if (AudioManager.Instance.MusicOn)
            {
                musicButton.text = "MUSIC: ON ";
            }
            else
            {
                musicButton.text = "MUSIC: OFF";
            }

            if (AudioManager.Instance.SfxOn)
            {
                sfxButton.text = "SOUND EFFECTS: ON ";
            }
            else
            {
                sfxButton.text = "SOUND EFFECTS: OFF";
            }
        }

    }

    public void MuteMusic()
    {
        if (AudioManager.Instance != null)
        {
            if (clickSound != null)
            {
                AudioManager.Instance.PlaySound(clickSound, Vector3.zero);
            }
            AudioManager.Instance.MuteMusic();
        }
    }

    public void MuteSfx()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.MuteSfx();
            if (clickSound != null)
            {
                AudioManager.Instance.PlaySound(clickSound, Vector3.zero);
            }
        }
    }

    void Update()
    {
        if (AudioManager.Instance != null)
        {
            if (AudioManager.Instance.MusicOn)
            {
                musicButton.text = "MUSIC: ON ";
            }
            else
            {
                musicButton.text = "MUSIC: OFF";   
            }

            if (AudioManager.Instance.SfxOn)
            {
                sfxButton.text = "SOUND EFFECTS: ON ";
            }
            else
            {
                sfxButton.text = "SOUND EFFECTS: OFF";
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
