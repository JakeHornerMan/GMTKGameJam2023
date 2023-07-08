using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    private SoundManager soundManager;
    public float removeTime = 12f;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Start()
    {
        IEnumerator coroutine = WaitAndDie(removeTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndDie(float dieTime)
    {
        yield return new WaitForSeconds(dieTime);
        removeToken();
    }

    public void tokenCollected(){
        soundManager.PlaySound(SoundManager.SoundType.PowerUp);
        removeToken();
    }

    public void removeToken(){
        Destroy(gameObject);
    }

}
