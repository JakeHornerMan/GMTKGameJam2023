using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSpawner : MonoBehaviour
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
        // PopulateLanesArray();
        lanes = GameProgressionValues.LaneMap;
        
        if(lanes.Count <= 0) PopulateLanesArray();

        GenerateRoad();
    }

    public bool IsthereRoadHere(int start, int end){
        for(int i = start; i <= end; i++){
            if(lanes[i].name == "RoadLane")
                return true;
        }
        return false;
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

        if(!IsthereRoadHere(1,3)){
            int randomNumber = Random.Range(1, 3);
            lanes[randomNumber] = laneTypes[0];
        }

        if(!IsthereRoadHere(9, 10)){
            int randomNumber = Random.Range(9, 10);
            lanes[randomNumber] = laneTypes[0];
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

        if(riverCount < 1){
            do{
                int randomNumber;
                bool isWater = true;
                do{
                    randomNumber = Random.Range(1, 10);
                    if(lanes[randomNumber].name != "RoadLane" && lanes[randomNumber].name != "River"){
                        isWater = false;
                    }
                }
                while(isWater);
                lanes[randomNumber] = laneTypes[5];
                riverCount++;
            }
            while(riverCount < 1);
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
