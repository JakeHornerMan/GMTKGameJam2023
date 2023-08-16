using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnBecameInvisible()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        gameObject.transform.parent.gameObject.GetComponent<Car>().CarGoesOffscreen();
    }

}
