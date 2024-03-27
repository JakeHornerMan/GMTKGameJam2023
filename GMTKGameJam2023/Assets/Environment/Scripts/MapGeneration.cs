using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGeneration : MonoBehaviour
{
    public List<GameObject> laneTypes;
    public int roadCount = 0;
    public int busCount = 0;
    public int dirtCount = 0;
    public int grassCount = 0;
    public int riverCount = 0;
    public int pavementCount = 0;

    public List<GameObject> lanes;
    
    private void Awake()
    {
        // GenerateRoad();
    }

    private void Start(){
        PopulateLanesArray();
    }

    public void PopulateLanesArray(){
        for (int i = 0; i < 12; i++){
            GameObject laneSelected;
            if(i == 0 || i == 11){
                laneSelected = laneTypes[1];
                lanes.Add(laneSelected);
                continue;
            }
            int randomNumber = Random.Range(0, laneTypes.Count);
            laneSelected = laneTypes[randomNumber];
            lanes.Add(laneSelected);
        }

        LaneCount();

        if(roadCount < 3){
            do{
                int randomNumber = Random.Range(0, 10);
                lanes[randomNumber] = laneTypes[0];
                roadCount++;
            }
            while(roadCount != 3);
            LaneCount();
        }
    }

    private void LaneCount(){
        roadCount = 0;
        busCount = 0;
        dirtCount = 0;
        grassCount = 0;
        riverCount = 0;
        pavementCount = 0;

        foreach (GameObject item in lanes)
        {
            switch (item.name)
            {
                case "BusLane":
                    busCount++;
                break;
                case "Dirt":
                    dirtCount++;
                break;
                case "GrassLane":
                    grassCount++;
                break;
                case "GrassLane_2":
                    grassCount++;
                break;
                case "PavementLane":
                    pavementCount++;
                break;
                case "River":
                    riverCount++;
                break;
                case "RoadLane":
                    roadCount++;
                break;
                default:
                    Debug.Log("Error with Gameobject Lane!");
                break;
            }
        }
    }

    

    // void GenerateRoad()
    // {
    //     for (int i = 0; i <= 10; i++)
    //     {
    //         GameObject road = roads[Random.Range(1, roads.Length)];
    //         Vector3 pos;
    //         switch (i)
    //         {
    //             case 0:
    //                 road = roads[2];
    //                 pos = new Vector3(-13f, 0, 0);
    //                 break;
    //             case 1:
    //                 pos = new Vector3(-10.5f, 0, 0);
    //                 break;
    //             case 2:
    //                 pos = new Vector3(-8f, 0, 0);
    //                 break;
    //             case 3:
    //                 pos = new Vector3(-5.5f, 0, 0);
    //                 break;
    //             case 4:
    //                 pos = new Vector3(-3f, 0, 0);
    //                 break;
    //             case 5:
    //                 pos = new Vector3(-0.5f, 0, 0);
    //                 break;
    //             case 6:
    //                 pos = new Vector3(2f, 0, 0);
    //                 break;
    //             case 7:
    //                 pos = new Vector3(4.5f, 0, 0);
    //                 break;
    //             case 8:
    //                 pos = new Vector3(7f, 0, 0);
    //                 break;
    //             case 9:
    //                 pos = new Vector3(9.5f, 0, 0);
    //                 break;
    //             case 10:
    //                 pos = new Vector3(12f, 0, 0);
    //                 break;
    //             // case 11:
    //             //      pos = new Vector3(14.5f,0,0);
    //             // break;
    //             // case 12:
    //             //      pos = new Vector3(17f,0,0);
    //             // break;
    //             default:
    //                 pos = new Vector3(0, 0, 0);
    //                 break;
    //         }
    //         Instantiate(road, pos, Quaternion.identity);
    //     }
    // }
}

[System.Serializable]
public class RoadRow
{
    public Vector3 position;
    public GameObject row;
}
