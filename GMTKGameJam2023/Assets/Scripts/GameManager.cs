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
    
    private void Start()
    {
        safelyCrossedChickens = 0;
        killCount = 0;
        playerScore = 0;
        tokens = 0;
    }
}
