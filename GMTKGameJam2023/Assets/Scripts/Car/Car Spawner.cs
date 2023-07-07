using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{

    [SerializeField] private GameObject carPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 spawnPoint = new Vector3(pos.x, 0, 0);
            Instantiate(carPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
