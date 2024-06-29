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
    [SerializeField] private Toggle fullscreenCheckbox;

    public static bool MusicEnabled;
    public static float MusicVolume;
    public static float SfxVolume;
    public static bool SfxEnabled;
    public static bool RoadShineEnabled;
    public static bool FullscreenEnabled;
    public static Vector2 WindowedSize;

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
        SfxVolume = 10;
        FullscreenEnabled = true;
        WindowedSize = new Vector2(1920f,1080f);

        SaveSettings();

        MatchUIToVariables();
        Fullscreen(FullscreenEnabled);
    }

    private void MatchUIToVariables()
    {
        // LoadSettings();

        musicCheckbox.isOn = MusicEnabled;
        musicVolumeSlider.value = MusicVolume;

        sfxCheckbox.isOn = SfxEnabled;
        sfxVolumeSlider.value = SfxVolume;

        roadShineCheckbox.isOn = RoadShineEnabled;
        fullscreenCheckbox.isOn = FullscreenEnabled;
        // Fullscreen(FullscreenEnabled);
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

    public void FullScreenToggle()
    {
        FullscreenEnabled = fullscreenCheckbox.isOn;
        Fullscreen(fullscreenCheckbox.isOn);
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

    public void Fullscreen(bool answer)
    {
        if(answer){
            int width = Screen.width;
            int height = Screen.height;
            WindowedSize = new Vector2(width, height);
            Screen.SetResolution(1920, 1080, true);
        }

        if(!answer){
            Screen.SetResolution((int)WindowedSize.x, (int)WindowedSize.y, true);
        }
        Screen.fullScreen = answer;
    }

    public static void SaveSettings(){
        SettingsData settingsData = new SettingsData(SfxEnabled, SfxVolume, MusicEnabled, MusicVolume, RoadShineEnabled, FullscreenEnabled, WindowedSize);
        string jsonData = JsonUtility.ToJson(settingsData);
        string filePath = Application.persistentDataPath + "/SettingData.json";
        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Settings saved to: "+ filePath);
    }

    public void LoadSettings(){
        string filePath = Application.persistentDataPath + "/SettingData.json";
        try{
            string jsonData = System.IO.File.ReadAllText(filePath);

            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(jsonData);

            // if(settingsData == null){
            //    ResetToDefault();
            //    return; 
            // }

            Settings.SfxEnabled = settingsData.SfxEnabled;
            Settings.SfxVolume = settingsData.SfxVolume;
            Settings.MusicEnabled = settingsData.MusicEnabled;
            Settings.MusicVolume = settingsData.MusicVolume;
            Settings.RoadShineEnabled = settingsData.RoadShineEnabled;
            Settings.FullscreenEnabled = settingsData.FullscreenEnabled;
            Settings.WindowedSize = settingsData.WindowedSize;

            ToString();
            MatchUIToVariables();
            Fullscreen(settingsData.FullscreenEnabled);
        }
        catch(System.Exception e){
            Debug.Log("Error finding file: "+ filePath);
            ResetToDefault();
            return; 
        }
        
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
    public bool FullscreenEnabled;
    public Vector2 WindowedSize;

     public SettingsData() { }

    public SettingsData(bool sfxEnabled, float sfxVolume, bool musicEnabled, float musicVolume, bool roadShineEnabled, bool fullscreenEnabled, Vector2 windowSize)
    {
        MusicEnabled = musicEnabled;
        MusicVolume = musicVolume;
        SfxEnabled = sfxEnabled;
        SfxVolume = sfxVolume;
        RoadShineEnabled = roadShineEnabled;
        FullscreenEnabled = fullscreenEnabled;
        WindowedSize = windowSize;
    } 

}
