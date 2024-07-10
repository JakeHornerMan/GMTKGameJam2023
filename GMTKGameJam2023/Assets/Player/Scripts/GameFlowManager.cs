using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private float emptyWaveTime;
    [SerializeField] private SpecialChicken[] SpecialChickens;
    [SerializeField] private List<SpecialChicken> chickenPot;
    [SerializeField] private List<SpecialChicken> potCandidates;
    [SerializeField] public ChickenWave[] bonusWaves;
    [SerializeField] private List<WavePrompt> prompts;
    private GameManager gameManager;

    private int standardChickenAmount = 0;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void newRound(){
        setStandardAmount();
        SetSpecialChickenCandidates();

        for(int i =1; i <= 4; i++){
            if(i == 2){
                SpecialWave();
            }
            else if(i == 4){
                EmptyWave();
            }
            else{
                StandardChickenWaveCreate(i);
            }
        }
    }

    private void StandardChickenWaveCreate(int waveNumber){
        ChickenWave newChickenWave = new ChickenWave();
        newChickenWave.roundTime = GameProgressionValues.standardRoundTime;
        newChickenWave.standardChickenAmounts = standardChickenAmount;

        // if(waveNumber == 1){
        //     newChickenWave.wavePrompt = "Level: " + GameProgressionValues.RoundNumber;
        // }
        // else {
            // newChickenWave.wavePrompt = "Chicken Amounts: " + newChickenWave.standardChickenAmounts;
            //TODO: insert funny prompts funtiality
            newChickenWave.wavePrompt = SetWavePrompt(newChickenWave.standardChickenAmounts);
        // }
        newChickenWave.chickenIntesity = chickenIntesitySet();
        newChickenWave.coinAmount = RoundCoinSet();
        newChickenWave.specialChickens = SpecialChickenListSet();
        gameManager.waves.Add(newChickenWave);
        // Debug.Log(newChickenWave.wavePrompt);
    }

    public string SetWavePrompt(int amount){
        int getPromptAt = Random.Range(0, prompts.Count-1);
        string line = prompts[getPromptAt].prompt;
        string answer;
        if(!prompts[getPromptAt].includesNum){
            prompts.RemoveAt(getPromptAt);
            answer = line;
        }
        else{
            answer = line.Replace("#", amount.ToString());
        }
        Debug.Log(answer);
        return answer;
    }

    private void setStandardAmount(){
        if(GameProgressionValues.RoundNumber > 25){
            standardChickenAmount  = Random.Range(330, 600);
        }
        else{
            standardChickenAmount = GameProgressionValues.standardChickenAmountForStart;
            for(int i = 2; i <= GameProgressionValues.RoundNumber; i++){
                int multiplier = (int)Mathf.Floor((float)i/5f);
                // Debug.Log("multi: " + multiplier);
                standardChickenAmount += 10 + (5 * multiplier);
            }
        } 
    }

    private void SetSpecialChickenCandidates()
    {
         //find amount of point for the round
        int chickenPoints = GameProgressionValues.RoundNumber*2;
        //new list to store potential chickens theat we can use
        foreach (SpecialChicken chic in chickenPot){
            if(chic.difficultyRank <= chickenPoints){
                potCandidates.Add(chic.DeepClone());
            }
        }
    }

    private List<SpecialChicken> SpecialChickenListSet(){
        List<SpecialChicken> specialChickensList = new List<SpecialChicken>();

        //find amount of point for the wave
        float multiplier;
        switch(GameProgressionValues.RoundNumber){
            case int r when (r < 5):
                multiplier = 2;
                break;
            case int r when (r < 10):
                multiplier = 2.5f;
                break;
            case int r when (r < 15):
                multiplier = 3;
                break;
            case int r when (r < 20):
                multiplier = 3.5f;
                break;
            case int r when (r < 25):
                multiplier = 4f;
                break;
            case int r when (r < 30):
                multiplier = 4.5f;
                break;
            case int r when (r > 30):
                multiplier = 5f;
                break;
            default:
                multiplier = 2;
                break;
        }

        int chickenPoints = (int)Mathf.Round(GameProgressionValues.RoundNumber*multiplier);
        int turboCount = 0;
        int rocketCount = 0;
        int toughGuyLimit = Mathf.FloorToInt(multiplier);

        //find random chickens in candidates for our wave
        do{
            int getChickenAt = Random.Range(0, potCandidates.Count-1);
            SpecialChicken specialChicken = potCandidates[getChickenAt].DeepClone();

            if(specialChicken.chicken.name.Contains("Turbo")){
                if(turboCount >= toughGuyLimit) continue;
                turboCount++;
            }
            if(specialChicken.chicken.name.Contains("Rocket")){
                if(rocketCount >= toughGuyLimit) continue;
                rocketCount++;
            }

            if(specialChicken.difficultyRank <= chickenPoints){
                specialChicken.timeToSpawn = Random.Range(1,GameProgressionValues.standardRoundTime-1);
                chickenPoints = chickenPoints - specialChicken.difficultyRank;
                specialChickensList.Add(specialChicken);
            }
        }
        while(chickenPoints > 0);

        return specialChickensList;
    } 

    private int RoundCoinSet(){
        if(GameProgressionValues.RoundNumber > 19){
            return 50;
        }
        return 30 + (GameProgressionValues.RoundNumber);
    }

    private int chickenIntesitySet(){
        int intensity = 1;
        if(GameProgressionValues.RoundNumber <= 5){
            intensity = Random.Range(1,3);
        }
        if(GameProgressionValues.RoundNumber <= 8){
            intensity = Random.Range(2,4);
        }
        if(GameProgressionValues.RoundNumber <= 12){
            intensity = Random.Range(3,5);
        }
        if(GameProgressionValues.RoundNumber <= 16){
            intensity = Random.Range(4,5);
        }
        if(GameProgressionValues.RoundNumber > 20){
            intensity = 5;
        }
        return intensity;
    }

    private void SpecialWave(){
        int getWave = Random.Range(0,bonusWaves.Length-1);
        ChickenWave copyWave = bonusWaves[getWave].DeepClone();
        copyWave.coinAmount = 20;
        gameManager.waves.Add(copyWave);
        // Debug.Log(copyWave.wavePrompt);
    }

    private void EmptyWave(){
        ChickenWave newChickenWave = new ChickenWave();
        newChickenWave.roundTime = emptyWaveTime;
        newChickenWave.standardChickenAmounts = 0;
        newChickenWave.wavePrompt = "Times Nearly Up!";
        newChickenWave.waveSound = "LastSeconds";
        newChickenWave.chickenIntesity = 0;
        newChickenWave.coinAmount = 0;
        newChickenWave.specialChickens = null;
        gameManager.waves.Add(newChickenWave);
        // Debug.Log(newChickenWave.wavePrompt);
    }
}

[System.Serializable]
public class ChickenWave
{
    public float roundTime;
    public string wavePrompt;
    public string waveSound;
    public int standardChickenAmounts;
    public int chickenIntesity = 0;
    public int coinAmount  = 0;
    public List<SpecialChicken> specialChickens;
    public ChickenWave() { }
    public ChickenWave(ChickenWave chickenWave) {
        roundTime = chickenWave.roundTime;
        wavePrompt = chickenWave.wavePrompt;
        standardChickenAmounts = chickenWave.standardChickenAmounts;
        chickenIntesity = chickenWave.chickenIntesity;
        coinAmount = chickenWave.coinAmount;
        specialChickens = new List<SpecialChicken>();
    }
    public ChickenWave DeepClone(){
        ChickenWave chickenWave = new ChickenWave();
        chickenWave = (ChickenWave)this.MemberwiseClone();
        chickenWave.specialChickens = new List<SpecialChicken>(specialChickens);

        return chickenWave;
    }
}

[System.Serializable]
public class SpecialChicken
{
    public float timeToSpawn;
    public GameObject chicken;
    public int difficultyRank;
    public bool topSpawn;
    public bool bottomSpawn;

    public SpecialChicken() { }
    public SpecialChicken(SpecialChicken specialChicken) {
        timeToSpawn = specialChicken.timeToSpawn;
        chicken = specialChicken.chicken;
        topSpawn = specialChicken.topSpawn;
        bottomSpawn = specialChicken.bottomSpawn;
        difficultyRank = specialChicken.difficultyRank;
    }

    public SpecialChicken DeepClone(){
        SpecialChicken specialChicken = new SpecialChicken();
        specialChicken.timeToSpawn = timeToSpawn;
        specialChicken.chicken = chicken;
        specialChicken.topSpawn = topSpawn;
        specialChicken.bottomSpawn = bottomSpawn;
        specialChicken.difficultyRank = difficultyRank;

        return specialChicken;
    }

}

[System.Serializable]
public class RankingRequirement
{
    public int minScore = 0;
    public string rankingString = "Poultry Terrorizer";
}

[System.Serializable]
public class WavePrompt
{
    public string prompt;
    public bool includesNum = false;
}