using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCorn : MonoBehaviour
{
    [SerializeField] private GameObject cornPrefab;
    [SerializeField] private Transform cornContainer;
    private GameManager gameManager;
    [HideInInspector] public List<GameObject> cornHealthList;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        CreateCorn();
    }

    private void CreateCorn()
    {
       GameObject corn;
        for (int i = 0; i < gameManager.missedChickenLives; i++)
        {
            corn = Instantiate(
                cornPrefab,
                cornContainer
            );
            cornHealthList.Add(corn);
        }
    }
}
