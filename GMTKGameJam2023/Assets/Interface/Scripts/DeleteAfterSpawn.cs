using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterSpawn : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float howLong = 2f;

    void Start()
    {
        StartCoroutine(WaitAndDelete(howLong));
    }

    private IEnumerator WaitAndDelete(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
