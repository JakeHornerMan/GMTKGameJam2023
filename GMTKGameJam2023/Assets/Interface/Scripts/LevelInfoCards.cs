using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelInfoCards : MonoBehaviour
{
    [SerializeField] private GameObject infoBox;
    [SerializeField] private List<GameObject> objectsForExplanation;
    [SerializeField] private List<GameObject> informationBoxes;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject container;
    private GameManager gameManager;

    // public GameObject infoTitle;

    public int currentBox = 0;

    private void Awake()
    {

    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (objectsForExplanation.Count > 0)
        {
            StopGame();
            CreateInfoBoxes();
        }
        else
        {
            StartGame();
        }
    }

    private void CreateInfoBoxes()
    {
        foreach (GameObject gameMechanic in objectsForExplanation)
        {
            GameObject createdInfoBox = Instantiate(infoBox, container.transform);
            PopulateInfoBox(createdInfoBox, gameMechanic);
        }
        // Debug.Log(informationBoxes[0].transform.Find("InfoTitle").gameObject.GetComponent<TextMeshProUGUI>().text);
        MoveInfoBoxRight2Mid(informationBoxes[0]);
    }

    private void PopulateInfoBox(GameObject infoBox, GameObject gameMechanic)
    {
        ObjectInfo info = gameMechanic.GetComponent<ObjectInfo>();

        GameObject populateItem = infoBox.transform.Find("InfoTitle").gameObject;
        populateItem.GetComponent<TextMeshProUGUI>().text = info.objectName;

        populateItem = infoBox.transform.Find("Image").gameObject;
        populateItem.GetComponent<Image>().sprite = info.objectIcon;

        populateItem = infoBox.transform.Find("Information").gameObject;
        populateItem.GetComponent<TextMeshProUGUI>().text = info.objectDescription;

        informationBoxes.Add(infoBox);
    }

    public void CloseInfoCards()
    {
        gameUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void StopGame()
    {
        // gameManager.pauseGameplay = true;
        gameUI.SetActive(false);
    }

    public void StartGame()
    {
        CloseInfoCards();
        // gameManager.pauseGameplay = false;
        // gameManager.SetStart();
    }

    public void MoveInfoBox(bool isRight)
    {
        if (isRight)
        {
            if (informationBoxes.Count <= currentBox + 1) return;

            MoveInfoBoxMid2Left(informationBoxes[currentBox]);
            currentBox++;
            MoveInfoBoxRight2Mid(informationBoxes[currentBox]);
        }
        else
        {
            if (currentBox == 0) return;
            MoveInfoBoxMid2Right(informationBoxes[currentBox]);
            currentBox--;
            MoveInfoBoxLeft2Mid(informationBoxes[currentBox]);
        }
    }

    private void MoveInfoBoxRight2Mid(GameObject box)
    {
        Animator anim = box.GetComponent<Animator>();
        anim.Play("Right2Mid");
    }

    private void MoveInfoBoxMid2Right(GameObject box)
    {
        Animator anim = box.GetComponent<Animator>();
        anim.Play("Mid2Right");
    }

    private void MoveInfoBoxLeft2Mid(GameObject box)
    {
        Animator anim = box.GetComponent<Animator>();
        anim.Play("Left2Mid");
    }

    private void MoveInfoBoxMid2Left(GameObject box)
    {
        Animator anim = box.GetComponent<Animator>();
        anim.Play("Mid2Left");
    }
}
