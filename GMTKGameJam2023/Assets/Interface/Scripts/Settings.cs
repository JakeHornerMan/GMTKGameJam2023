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
    public static float MusicVolume;
    public static float SfxVolume;
    public static bool SfxEnabled;
    public static bool RoadShineEnabled;

    [SerializeField] private SoundManager musicManager;
    [SerializeField] private SoundManager sfxManager;

    private void Awake()
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
        if(!musicCheckbox.isOn){
            musicManager.musicAudio.Pause();
        }
        else{
            musicManager.musicAudio.UnPause();
            musicManager.musicAudio.volume = MusicVolume/10f;
        }

        if(musicCheckbox.isOn && musicManager.musicAudio.clip == null){
            musicManager.PlayMusic();
        }
    }

    public void SFXToggle()
    {
        SfxEnabled = sfxCheckbox.isOn;
        if(!sfxCheckbox.isOn){
            sfxManager.sfxAllowed = false;
        }
        else{
            sfxManager.sfxAllowed = true;
            sfxManager.sfxVolume = SfxVolume/10f;
        }
    }

    public void RoadShineToggle()
    {
        RoadShineEnabled = roadShineCheckbox.isOn;
    }

    // Slider Update Functions
    public void MusicVolumeSlider()
    {
        MusicVolume = musicVolumeSlider.value;
        if(musicCheckbox.isOn){
            musicManager.musicAudio.volume = MusicVolume/10;
        }
    }

    public void SFXVolumeSlider()
    {
        SfxVolume = sfxVolumeSlider.value;
        if(!sfxCheckbox.isOn){
            sfxManager.sfxVolume = SfxVolume/10f;
        }
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
    public float SfxVolume;
    public bool SfxEnabled;
    public bool MusicEnabled;
    public float MusicVolume;
    public bool RoadShineEnabled;

     public SettingsData() { }

    public SettingsData(bool sfxEnabled, float sfxVolume, bool musicEnabled, float musicVolume, bool roadShineEnabled)
    {
        MusicEnabled = musicEnabled;
        MusicVolume = musicVolume;
        SfxEnabled = sfxEnabled;
        SfxVolume = sfxVolume;
        RoadShineEnabled = roadShineEnabled;
    } 

}
