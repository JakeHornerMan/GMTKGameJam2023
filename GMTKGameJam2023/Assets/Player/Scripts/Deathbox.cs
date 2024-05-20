using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathbox : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string chickenTag = "Chicken";

    private GameManager gameManager;
    private TutorialManager tutorialManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(chickenTag))
        {
            Destroy(other.transform.parent.gameObject);
            if(gameManager != null) gameManager.SafelyCrossedChicken();
            if(tutorialManager != null) tutorialManager.SafelyCrossedChicken();
        }
    }
}
