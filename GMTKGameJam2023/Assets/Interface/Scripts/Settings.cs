using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    private const string musicToggleKey = "MusicEnabled";
    private const string sfxToggleKey = "SFXEnabled";

    public static bool musicAllowed { get; private set; } = true;
    public static bool sfxAllowed { get; private set; } = true;

    private void Start()
    {
        SetDefaultSettings();
        UpdateToggles();
    }

    private void SetDefaultSettings()
    {
        // Check if PlayerPrefs keys exist
        if (!PlayerPrefs.HasKey(musicToggleKey))
        {
            // Set music toggle to enabled by default
            PlayerPrefs.SetInt(musicToggleKey, BoolToInt(true));
            musicToggle.isOn = true;
        }

        if (!PlayerPrefs.HasKey(sfxToggleKey))
        {
            // Set sound effects toggle to enabled by default
            PlayerPrefs.SetInt(sfxToggleKey, BoolToInt(true));
            sfxToggle.isOn = true;
        }
    }

    private void UpdateToggles()
    {
        // Update UI based on PlayerPrefs values
        musicToggle.isOn = IntToBool(PlayerPrefs.GetInt(musicToggleKey));
        sfxToggle.isOn = IntToBool(PlayerPrefs.GetInt(sfxToggleKey));

        // Update public static variables
        musicAllowed = musicToggle.isOn;
        sfxAllowed = sfxToggle.isOn;
    }

    // Checkbox Update Functions
    public void MusicToggle(bool on)
    {
        PlayerPrefs.SetInt(musicToggleKey, BoolToInt(on));
        UpdateToggles();
    }

    public void SFXToggle(bool on)
    {
        PlayerPrefs.SetInt(sfxToggleKey, BoolToInt(on));
        UpdateToggles();
    }

    // Utility Functions
    private int BoolToInt(bool x) => x ? 1 : 0;
    private bool IntToBool(int i) => i != 0;
}
