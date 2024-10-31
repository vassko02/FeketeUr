using System;

[System.Serializable]
public class Settings
{
    public float SFXVolume;
    public float MusicVolume;
    public string Resolution;

    public Settings(float sfxVolume, float musicVolume, string resolution)
    {
        SFXVolume = sfxVolume;
        MusicVolume = musicVolume;
        Resolution = resolution;
    }
    public Settings()
    {
            
    }
}