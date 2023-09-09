using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombingJet : Car
{
    [Header("Camera Shake")]
    [SerializeField] private float jetShakeDuration = 3f;
    [SerializeField] private float jetShakeMagnitude = 0.25f;

    [Header("Bombing Run Parameters")]
    [SerializeField] private float laneSpacing = 2f;
    [SerializeField] private float delayBeforeBombsAppear = 0.5f;

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float numberOfBombs = 4f;
    [SerializeField] private float laneLength = 17f;
    [SerializeField] private float delayBetweenBombs = 0.25f;

    private float numberOfBombsProcessed = 0;

    void Start()
    {
        SetCarSpeed();

        soundManager?.PlayNewFighterJet();

        // Shake Camera
        StartCoroutine(cameraShaker.Shake(jetShakeDuration, jetShakeMagnitude));

        StartCoroutine(WaitForBombStart());
    }



    IEnumerator WaitForBombStart()
    {
        yield return new WaitForSeconds(delayBeforeBombsAppear);

        StartCoroutine(DeployBombs());
    }

    private IEnumerator DeployBombs()
    {
        for (int i = 1; i < numberOfBombs + 1; i++)
        {
            float bombY = ((laneLength * -1) / 2) + (((laneLength / 4) * i) - laneLength / 8);

            float bombX = 0;

            for (int j = 0; j < 2; j++)
            {
                if (j == 0)
                {
                    bombX = gameObject.transform.position.x - laneSpacing;
                }
                else
                {
                    bombX = gameObject.transform.position.x + laneSpacing;
                }

                Vector3 bombPosition = new Vector3(bombX, bombY, 1);

                GameObject currentBomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);

                currentBomb.GetComponent<Bomb>().AssignJetParent(gameObject.GetComponent<BombingJet>());


            }

            yield return new WaitForSeconds(delayBetweenBombs);



        }

    }


    public override void CarGoesOffscreen()
    {

    }



    public void AddBombScore(int points)
    {
        totalPoints = totalPoints + points;


        numberOfBombsProcessed++;

        if (numberOfBombsProcessed == numberOfBombs * 2)
        {
            if (totalPoints > 0)
                gameManager.AddPlayerScore(totalPoints);
            Destroy(gameObject);
        }
    }
}
