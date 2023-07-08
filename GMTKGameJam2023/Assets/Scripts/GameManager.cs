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
    
    private void Start()
    {
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
        if(time % 30f == 0){
            Debug.Log("30 seconds");
        }
        if ( time <= 0 )
        {
            // send to results screen
        }
    }
}
