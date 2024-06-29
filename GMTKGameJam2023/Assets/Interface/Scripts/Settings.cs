using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Toggle musicCheckbox;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Toggle sfxCheckbox;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle roadShineCheckbox;

    public static bool MusicEnabled;
    public static int MusicVolume;
    public static int SfxVolume;
    public static bool SfxEnabled;
    public static bool RoadShineEnabled;

    private void Start()
    {
        LoadSettings();
    }

    public void ResetToDefault()
    {
        MusicEnabled = true;
        SfxEnabled = true;
        RoadShineEnabled = true;
        MusicVolume = 8;
        SfxVolume = 8;

        SaveSettings();

        MatchUIToVariables();
    }

    private void MatchUIToVariables()
    {
        // LoadSettings();

        musicCheckbox.isOn = MusicEnabled;
        musicVolumeSlider.value = MusicVolume;

        sfxCheckbox.isOn = SfxEnabled;
        sfxVolumeSlider.value = SfxVolume;

        roadShineCheckbox.isOn = RoadShineEnabled;
    }

    // Checkbox Update Functions
    public void MusicToggle()
    {
        MusicEnabled = musicCheckbox.isOn;
    }

    public void SFXToggle()
    {
        SfxEnabled = sfxCheckbox.isOn;
    }

    public void RoadShineToggle()
    {
        RoadShineEnabled = roadShineCheckbox.isOn;
    }

    // Slider Update Functions
    public void MusicVolumeSlider()
    {
        MusicVolume = (int)musicVolumeSlider.value;
    }

    public void SFXVolumeSlider()
    {
        SfxVolume = (int)sfxVolumeSlider.value;
    }

    // Utility Functions
    private int BoolToInt(bool x) => x ? 1 : 0;
    private bool IntToBool(int i) => i != 0;

    public static void SaveSettings(){
        SettingsData settingsData = new SettingsData(SfxEnabled, SfxVolume, MusicEnabled, MusicVolume, RoadShineEnabled);
        string jsonData = JsonUtility.ToJson(settingsData);
        string filePath = Application.persistentDataPath + "/SettingData.json";
        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Settings saved to: "+ filePath);
    }

    public void LoadSettings(){
        string filePath = Application.persistentDataPath + "/SettingData.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SettingsData settingsData = JsonUtility.FromJson<SettingsData>(jsonData);

        Settings.SfxEnabled = settingsData.SfxEnabled;
        Settings.SfxVolume = settingsData.SfxVolume;
        Settings.MusicEnabled = settingsData.MusicEnabled;
        Settings.MusicVolume = settingsData.MusicVolume;
        Settings.RoadShineEnabled = settingsData.RoadShineEnabled;

        ToString();
        MatchUIToVariables();
    }

    public void ToString(){
        Debug.Log("SfxEnabled: " + SfxEnabled +", SfxVolume: "+ SfxVolume + ", MusicEnabled: " + MusicEnabled + ", MusicVolume: " + MusicVolume + ", RoadShineEnabled: " + RoadShineEnabled);
    }
}

[System.Serializable]
public class SettingsData
{
    public int SfxVolume;
    public bool SfxEnabled;
    public bool MusicEnabled;
    public int MusicVolume;
    public bool RoadShineEnabled;

     public SettingsData() { }

    public SettingsData(bool sfxEnabled, int sfxVolume, bool musicEnabled, int musicVolume, bool roadShineEnabled)
    {
        MusicEnabled = musicEnabled;
        MusicVolume = musicVolume;
        SfxEnabled = sfxEnabled;
        SfxVolume = sfxVolume;
        RoadShineEnabled = roadShineEnabled;
    } 

}
