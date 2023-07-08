using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int safelyCrossedChickens = 0;
    public int killCount = 0;
    public int playerScore = 0;
    public int tokens = 0;
    public float time = 120f;
    public int intesitySetting = 0;

    private SoundManager soundManager;
    
    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        safelyCrossedChickens = 0;
        killCount = 0;
        playerScore = 0;
        tokens = 0;
    }
    private void Update() {
        setTime();
    }

    private void setTime(){
        time -= Time.deltaTime;
        if(time <= 90f && intesitySetting == 0){
            intesitySetting ++;
            // Debug.Log("90 seconds");
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
        }
        if(time <= 60f && intesitySetting == 1){
            intesitySetting ++;
            // Debug.Log("60 seconds");
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
        }
        if(time <= 40f && intesitySetting == 2){
            intesitySetting ++;
            // Debug.Log("40 seconds");
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
        }
        if(time <= 20f && intesitySetting == 3){
            intesitySetting ++;
            // Debug.Log("20 seconds");
            soundManager.PlaySound(SoundManager.SoundType.GameSpeed);
        }
        if ( time <= 0)
        {
            // send to results screen
        }
    }
}
