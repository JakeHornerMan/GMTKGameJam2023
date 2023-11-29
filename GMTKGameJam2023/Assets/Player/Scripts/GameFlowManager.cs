using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] public List<SpecialChicken> chickenPot;
    [SerializeField] public ChickenWave[] bonusWaves;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void newRound(){
        // RoundProgression();
        for(int i =0; i < 4; i++){
            if(i == 1){
                SpecialWave();
            }
            else{
                StandardChickenWaveCreate();
            }
        }
        GameProgressionValues.RoundNumber ++;
    }

    private void StandardChickenWaveCreate(){
        ChickenWave newChickenWave = new ChickenWave();
        newChickenWave.roundTime = GameProgressionValues.standardRoundTime;
        newChickenWave.wavePrompt = "Lets Go!";
        newChickenWave.standardChickenAmounts = standardChickenAmount();
        // newChickenWave.standardChickenAmounts = (int)(GameProgressionValues.standardChickenAmountForLevel);
        newChickenWave.wavePrompt = "Chciken Amounts: " + newChickenWave.standardChickenAmounts;
        newChickenWave.chickenIntesity = Random.Range(1,5);
        newChickenWave.coinAmount = RoundCoinSet();
        newChickenWave.specialChickens = null;
        gameManager.waves.Add(newChickenWave);
    }

    private int standardChickenAmount(){
        // GameProgressionValues.standardChickenAmountForLevel = GameProgressionValues.standardChickenAmountForLevel * GameProgressionValues.standardChickenAmountMultiplier;
        float amount = GameProgressionValues.standardChickenAmountForStart;
        if(GameProgressionValues.RoundNumber <= 30){
            for(int i = 0; i < GameProgressionValues.RoundNumber; i ++)
            {
                amount = amount * GameProgressionValues.standardChickenAmountMultiplier;
            }
        }
        else{
            int rando = Random.Range(15,30);
            for(int i = 0; i < rando; i ++)
            {
                amount = amount * GameProgressionValues.standardChickenAmountMultiplier;
            }
        }
        return (int)amount;
    }

    private void SpecialChickenListSet(ChickenWave newChickenWave){
        //need to add special chicken progression here
    } 

    private int RoundCoinSet(){
        //need to make this smarter
        return 15;
    }

    private void SpecialWave(){
        int getWave = Random.Range(0,bonusWaves.Length);
        gameManager.waves.Add(bonusWaves[getWave]);
    }
}
