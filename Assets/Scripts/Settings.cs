using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Settings : MonoBehaviour
{
    public TMP_InputField nameField;
    public Slider sfxField;
    public Slider musicField;
    public TMP_Dropdown resolutionField;
    public Toggle FullScreenField;
    public AudioMixer sfxMixer;
    public AudioMixer musicMixer;

    Resolution[] resolutions;

    private void Start()
    {

        LoadResolutions();
    }
    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("volume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("volume", volume);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    private void LoadResolutions()
    {
        int currentResolutioIndex = 0;
        resolutions = Screen.resolutions;
        resolutionField.ClearOptions();
        List<string> resOptions = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string format = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(format);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutioIndex = i;
            }
        }
        resolutionField.AddOptions(resOptions);
        resolutionField.value = currentResolutioIndex;
        resolutionField.RefreshShownValue();
    }

    public void SaveSettings()
    {
        //Save
        SceneManager.LoadScene("MainMenu");
    }
}
