using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierMenu : MonoBehaviour
{
    public float damage = 50f;
    private Animator anim;
    private SoundManager soundManager;
    private float wallDisappearAnimLength = 1f;
    private Collider2D collider;

    void Start()
    {
        anim = GetComponent<Animator>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    public void BarrierHit()
    {
        StartCoroutine(WaitAndDie(1f));
    }

    private IEnumerator WaitAndDie(float dieTime)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        anim.Play("Break");

        yield return new WaitForSeconds(wallDisappearAnimLength);

        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
