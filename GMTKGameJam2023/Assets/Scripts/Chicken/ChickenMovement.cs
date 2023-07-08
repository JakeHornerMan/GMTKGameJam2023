using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    float moveTime;

    private Rigidbody2D rb;

    public float minMoveTime = 0.5f;
    public float maxMoveTime = 3f;
    public float chickenSpeed = 1f;
    public float laneDistance = 2f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        moveTime = Random.Range(minMoveTime, maxMoveTime);
        // Debug.Log("Time: " + moveTime);
        startMovement();
    }

    void startMovement(){
        IEnumerator coroutine = WaitAndMove(moveTime);
        StartCoroutine(coroutine);
    }

    IEnumerator WaitAndMove(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);
        moveChicken();
        //restart timer
        startMovement();
    }

    void moveChicken(){
        // Debug.Log("Moving");
        Vector2 targetPoint = rb.position + new Vector2(laneDistance, 0f);
        // Vector2 direction = targetPoint - rb.position;
        // direction.Normalize();

        // Vector2 movement = direction * chickenSpeed * Time.deltaTime;
        // transform.pranslate(movement);
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, chickenSpeed);
    }

    public void KillChicken(){
        GameObject soundmanager = GameObject.Find("GameManger");
        soundmanager.GetComponent<SoundManager>().PlaySound("death");
        Destroy(this.gameObject);
    }
}
