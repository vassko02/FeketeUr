using System;

[System.Serializable]
public class Settings
{
    public float SFXVolume;
    public float MusicVolume;
    public int Resolution;
    public string name;
    public bool fullScreen;

    public Settings(float sfxVolume, float musicVolume, int resolution, string name, bool fullScreen)
    {
        SFXVolume = sfxVolume;
        MusicVolume = musicVolume;
        Resolution = resolution;
        this.name = name;
        this.fullScreen = fullScreen;
    }
    public Settings()
    {
            
    }
}