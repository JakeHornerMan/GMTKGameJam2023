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
        for (int i = 0; i < gameManager.missedChickenLives; i++)
        {
            // StartCoroutine(WaitTime(1f));
            GameObject corn;
            corn = Instantiate(cornPrefab, cornContainer) as GameObject;
            cornHealthList.Add(corn);
        }
    }

    public IEnumerator WaitTime(float stopTime)
    {
        yield return new WaitForSeconds(stopTime);
    }

    public void DeadCorn(int health){
        GameObject corn = cornHealthList[health];
        corn.GetComponent<Animator>().Play("DisabledCorn");
    }
}
