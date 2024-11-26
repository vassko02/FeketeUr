using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    SaveManager saveManager => SaveManager.Instance;
    public TMP_InputField nameField;
    public Slider sfxField;
    public Slider musicField;
    public TMP_Dropdown resolutionField;
    public Toggle FullScreenField;
    public AudioMixer sfxMixer;
    public AudioMixer musicMixer;

    List<Resolution> resolutions;

    private void Start()
    {
        LoadResolutions();
        LoadData();
    }

    private void LoadData()
    {
        if (saveManager.saveData.settingsData!=new Settings())
        {
            nameField.text = saveManager.saveData.settingsData.name;
            sfxField.value = saveManager.saveData.settingsData.SFXVolume;
            musicField.value = saveManager.saveData.settingsData.MusicVolume;
            resolutionField.value = saveManager.saveData.settingsData.Resolution;
            FullScreenField.isOn = saveManager.saveData.settingsData.fullScreen;
        }
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
        resolutions = Screen.resolutions.ToList();
        resolutionField.ClearOptions();
        List<string> resOptions = new List<string>();
        for (int i = 0; i < resolutions.Count; i++)
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
        saveManager.saveData.settingsData.name = nameField.text;
        saveManager.saveData.settingsData.fullScreen = FullScreenField.isOn;
        saveManager.saveData.settingsData.SFXVolume = sfxField.value;
        saveManager.saveData.settingsData.MusicVolume = musicField.value;
        saveManager.saveData.settingsData.Resolution = resolutionField.value;
        saveManager.Save();
        SceneManager.LoadScene("MainMenu");
    }
}