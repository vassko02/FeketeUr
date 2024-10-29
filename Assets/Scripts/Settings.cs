public class Settings
{
    public float SFXVolume { get; set; }
    public float MusicVolume { get; set; }
    public string Resolution { get; set; }

    public Settings(float sfxVolume, float musicVolume, string resolution)
    {
        SFXVolume = sfxVolume;
        MusicVolume = musicVolume;
        Resolution = resolution;
    }
}