using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] public int safelyCrossedChickens = 0;
    [SerializeField] public int playerScore = 0;

    private void Start()
    {
        safelyCrossedChickens = 0;
        playerScore = 0;
    }
}
