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
        // GameProgressionValues.ResetLaneMap();
        PopulateLanesArray();
        // GenerateRoad();
    }

    public void ShowInfo(){
        FindObjectOfType<ObjectBlueprint>(true).DisplayInfo(lanes);
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

        GameProgressionValues.SetLaneMap(lanes);

        // foreach(GameObject lane in GameProgressionValues.LaneMap){
        //     Debug.Log(lane.name);
        // }
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

    

    // private void GenerateRoad()
    // {
    //     float instantiatePosX = -13.4f;
    //     for (int i = 0; i < lanes.Count; i++)
    //     {
    //         Vector3 pos = new Vector3(instantiatePosX, 0, 0);
    //         Instantiate(lanes[i], pos, Quaternion.identity, laneContainer.transform);
    //         instantiatePosX = instantiatePosX + 2.5f;
    //     }
    // }
}