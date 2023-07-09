using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject[] roads;

    public RoadRow[] roadRows = new RoadRow[12];

    void Awake()
    {
        GenerateRoad();
    }

    void GenerateRoad()
    {
        for (int i =0; i <= 10; i++)
        {
            GameObject road = roads[Random.Range(1, roads.Length)];
            Vector3 pos;
            switch (i) 
            {
                case 0:
                    road = roads[2];
                    pos = new Vector3(-13f,0,0);
                break;
                case 1:
                    pos = new Vector3(-10.5f,0,0);
                break;
                case 2:
                    pos = new Vector3(-8f,0,0);
                break;
                case 3:
                    pos = new Vector3(-5.5f,0,0);
                break;
                case 4:
                    pos = new Vector3(-3f,0,0);
                break;
                case 5:
                     pos = new Vector3(-0.5f,0,0);
                break;
                case 6:
                     pos = new Vector3(2f,0,0);
                break;
                case 7:
                     pos = new Vector3(4.5f,0,0);
                break;
                case 8:
                     pos = new Vector3(7f,0,0);
                break;
                case 9:
                     pos = new Vector3(9.5f,0,0);
                break;
                case 10:
                     pos = new Vector3(12f,0,0);
                break;
                // case 11:
                //      pos = new Vector3(14.5f,0,0);
                // break;
                // case 12:
                //      pos = new Vector3(17f,0,0);
                // break;
                default:
                     pos = new Vector3(0,0,0);
                break;
            }
            Instantiate(road, pos, Quaternion.identity);
        }
    }
}

[System.Serializable]
public class RoadRow 
{
    public Vector3 position;
    public GameObject row;
}
