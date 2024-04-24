using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRoad : MonoBehaviour
{
    public string roadName;

    [SerializeField] public GameObject roadHighlight;


    private Animator roadHighlightAnim;

    private GameObject chickenParent;

    [SerializeField] private GameObject[] bonusObjects;

    // Start is called before the first frame update
    void Start()
    {
        roadHighlight = transform.GetChild(2).gameObject;

        roadHighlightAnim = roadHighlight.GetComponent<Animator>();

        chickenParent = transform.GetChild(3).gameObject;
    }

    public void ActivateHighlight()
    {
        
        roadHighlight.SetActive(true);

        roadHighlightAnim.SetTrigger("StartHighlight");

        
    }

    public void DeactivateHighlight()
    {
        roadHighlightAnim.SetTrigger("StopHighlight");

        //roadHighlight.SetActive(false);
    }

    public void ResetRoad()
    {
        ActivateChicken();

        for (int i = 0; i < bonusObjects.Length; i++)
        {
            bonusObjects[i].SetActive(true);
        }

    }
    public void ActivateChicken()
    {
        //if (chickenParent.transform.childCount == 0)
        //{
        //    Instantiate(chickenPrefab, transform.position, Quaternion.identity, chickenParent.transform);
        //}
        
        chickenParent.transform.GetChild(0).gameObject.SetActive(true);

    }
}
