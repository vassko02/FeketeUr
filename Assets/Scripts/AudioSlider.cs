using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private AudioSource AudioSource;
    [SerializeField]
    private TextMeshProUGUI ValueText;
    [SerializeField]
    private Slider volumeSlider;

    private float currentVolume;
    private const string VolumePrefKey = "AudioVolume"; // Ezen a key-en �rthet� el a men� zene hanger�


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // Load the saved volume when the game starts
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
        SetVolume(savedVolume);
    }

    public void OnChangeSlider(float value)
    {
        // Friss�ti a hanger�t a slider mozg�sra
        currentVolume = value;

        // Be�ll�tja a mixeren a hanger�t
        SetVolume(currentVolume);
    }

    public void SaveSettings()
    {
        // Elmenti a be�ll�t�sokat
        PlayerPrefs.SetFloat(VolumePrefKey, currentVolume);
        PlayerPrefs.Save();
    }

    public void Quit()
    {
        // Vissza�ll�tja az eredeti �rt�kre a be�ll�t�sokat
        float originalVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
        PlayerPrefs.SetFloat(VolumePrefKey, originalVolume);
        Mixer.SetFloat("Volume", Mathf.Log10(originalVolume) * 20);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void SetVolume(float value)
    {
        Mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        volumeSlider.value = value;
    }

}
