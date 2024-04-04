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

    private GameObject laneContainer;
    
    private void Awake()
    {
        laneContainer = GameObject.Find("Lanes");
        PopulateLanesArray();
        GenerateRoad();
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

        if(roadCount < 4){
            do{
                int randomNumber;
                bool isRoad = true;
                do{
                    randomNumber = Random.Range(1, 10);
                    if(lanes[randomNumber].name != "RoadLane"){
                        isRoad = false;
                    }
                }
                while(isRoad);
                lanes[randomNumber] = laneTypes[0];
                roadCount++;
            }
            while(roadCount != 4);
            LaneCount();
        }

        if(busCount >= 2){
            for(int i = 0; i < lanes.Count; i++){
                if(lanes[i].name == "BusLane"){
                    lanes[i] = laneTypes[3];
                    busCount--;
                }
                if(busCount == 1){
                    break;
                }
            } 
            LaneCount();
        }

        if(pavementCount >= 3){
            for(int i = 0; i < lanes.Count; i++){
                if(lanes[i].name == "PavementLane"){
                    lanes[i] = laneTypes[2];
                    pavementCount--;
                }
                if(pavementCount == 2){
                    break;
                }
            } 
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

    

    private void GenerateRoad()
    {
        float instantiatePosX = -13.4f;
        for (int i = 0; i < lanes.Count; i++)
        {
            Vector3 pos = new Vector3(instantiatePosX, 0, 0);
            Instantiate(lanes[i], pos, Quaternion.identity, laneContainer.transform);
            instantiatePosX = instantiatePosX + 2.5f;
        }
    }
}

// GameObject road = roads[Random.Range(1, roads.Length)];
//             Vector3 pos;
//             switch (i)
//             {
//                 case 0:
//                     pos = new Vector3(-13f, 0, 0);
//                     break;
//                 case 1:
//                     pos = new Vector3(-10.5f, 0, 0);
//                     break;
//                 case 2:
//                     pos = new Vector3(-8f, 0, 0);
//                     break;
//                 case 3:
//                     pos = new Vector3(-5.5f, 0, 0);
//                     break;
//                 case 4:
//                     pos = new Vector3(-3f, 0, 0);
//                     break;
//                 case 5:
//                     pos = new Vector3(-0.5f, 0, 0);
//                     break;
//                 case 6:
//                     pos = new Vector3(2f, 0, 0);
//                     break;
//                 case 7:
//                     pos = new Vector3(4.5f, 0, 0);
//                     break;
//                 case 8:
//                     pos = new Vector3(7f, 0, 0);
//                     break;
//                 case 9:
//                     pos = new Vector3(9.5f, 0, 0);
//                     break;
//                 case 10:
//                     pos = new Vector3(12f, 0, 0);
//                     break;
//                 default:
//                     Debug.Log("Issue with Instantiation!");
//                     break;
//             }

[System.Serializable]
public class RoadRow
{
    public Vector3 position;
    public GameObject row;
}
