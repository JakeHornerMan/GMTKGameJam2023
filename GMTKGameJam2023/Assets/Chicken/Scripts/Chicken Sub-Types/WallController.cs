using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    private Animator anim;
    private SoundManager soundManager;
    private float wallDisappearAnimLength = 1f;
    private Collider2D collider;

    void Start()
    {
        anim = GetComponent<Animator>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    public void WallHit()
    {
        StartCoroutine(WaitAndDie(1f));
    }

    private IEnumerator WaitAndDie(float dieTime)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        anim.Play("Break");

        yield return new WaitForSeconds(wallDisappearAnimLength);

        Destroy(gameObject);
    }
}
