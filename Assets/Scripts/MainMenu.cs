using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
    private const string VolumePrefKey = "AudioVolume";
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Level1");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadSceneAsync("Options");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f); // Default to 1 (max volume) if not set
        SetVolume(savedVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetVolume(float value)
    {
        Mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
    }
}
