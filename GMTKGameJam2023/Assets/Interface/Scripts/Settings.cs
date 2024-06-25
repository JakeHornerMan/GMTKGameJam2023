using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Toggle musicCheckbox;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Toggle sfxCheckbox;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle roadShineCheckbox;

    public static bool MusicEnabled { get; private set; } = true;
    public static int MusicVolume { get; private set; } = 10;
    public static int SfxVolume { get; private set; } = 10;
    public static bool SfxEnabled { get; private set; } = true;
    public static bool RoadShineEnabled { get; private set; } = true;

    private void Start()
    {
        MatchUIToVariables();
    }

    public void ResetToDefault()
    {
        MusicEnabled = true;
        SfxEnabled = true;
        RoadShineEnabled = true;
        MusicVolume = 10;
        SfxVolume = 10;

        MatchUIToVariables();
    }

    private void MatchUIToVariables()
    {
        musicCheckbox.isOn = MusicEnabled;
        musicVolumeSlider.value = MusicVolume;

        sfxCheckbox.isOn = SfxEnabled;
        sfxVolumeSlider.value = SfxVolume;

        roadShineCheckbox.isOn = RoadShineEnabled;
    }

    // Checkbox Update Functions
    public void MusicToggle(bool on)
    {
        MusicEnabled = on;
    }

    public void SFXToggle(bool on)
    {
        SfxEnabled = on;
    }

    public void RoadShineToggle(bool on)
    {
        RoadShineEnabled = on;
    }

    // Slider Update Functions
    public void MusicVolumeSlider(int value)
    {
        MusicVolume = value;
    }

    public void SFXVolumeSlider(int value)
    {
        SfxVolume = value;
    }

    // Utility Functions
    private int BoolToInt(bool x) => x ? 1 : 0;
    private bool IntToBool(int i) => i != 0;
}
