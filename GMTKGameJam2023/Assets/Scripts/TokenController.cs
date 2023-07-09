using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    [Header("Sway Values")]
    public float removeTime = 3f; 
    public float amplitude = 0.2f;     // The maximum distance of sway
    public float frequency = 20f;     // The frequency of the sway motion
    public float speed = 2f;         // The speed at which the object moves horizontally

    private SoundManager soundManager;
    private Animator anim;

    private Vector3 initialPosition;
   
    private float tokenShrinkAnimLength = 1.5f;

    void Awake()
    {
        initialPosition = transform.position;
        soundManager = FindObjectOfType<SoundManager>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        IEnumerator coroutine = WaitAndDie(removeTime);
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        float verticalDisplacement = amplitude * Mathf.Sin(Time.time * frequency);
        float horizontalDisplacement = speed * Time.deltaTime;
        Vector3 newPosition = initialPosition + new Vector3(horizontalDisplacement, verticalDisplacement, 0f);
        transform.Translate(newPosition - transform.position);
    }

    private IEnumerator WaitAndDie(float dieTime)
    {
        yield return new WaitForSeconds(dieTime - tokenShrinkAnimLength);

        anim.Play("Token Shrink");

        yield return new WaitForSeconds(tokenShrinkAnimLength);

        RemoveToken();
    }

    public void TokenCollected()
    {
        soundManager.PlaySound(SoundManager.SoundType.PowerUp);
        RemoveToken();
    }

    public void RemoveToken()
    {
        Destroy(gameObject);
    }
}
