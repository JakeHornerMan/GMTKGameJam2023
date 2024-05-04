using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCorn : MonoBehaviour
{
    [SerializeField] private GameObject cornPrefab;
    [SerializeField] private Transform cornContainer;
    public GameManager gameManager;
    public TutorialManager tutorialManager;
    [HideInInspector] public List<GameObject> cornHealthList;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    void Start()
    {
        if(gameManager != null){
            CreateCorn(gameManager.missedChickenLives);
        }
        if(tutorialManager != null){
            CreateCorn(tutorialManager.missedChickenLives);
        }
    }

    private void CreateCorn(int missedChickenLives)
    {
        for (int i = 0; i < missedChickenLives; i++)
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
