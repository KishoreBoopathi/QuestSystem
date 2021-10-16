using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    int experiencePoint = 0;
    int gold = 0;

    [SerializeField] Text experienceText;
    [SerializeField] Text goldText;

    private void Start() 
    {
        GameEvents.StartListening("RewardExperience", AddExpPoints);
        GameEvents.StartListening("RewardGold", AddGold);
        GameEvents.StartListening("ReduceExperience", ReduceExpPoints);
        GameEvents.StartListening("ReduceGold", ReduceGold);
    }
    void AddExpPoints(int rewardAmount)
    {
        experiencePoint += rewardAmount;
        experienceText.text = experiencePoint.ToString(); 
    }
    void AddGold(int rewardAmount)
    {
        gold += rewardAmount;
        goldText.text = gold.ToString();
    }
    void ReduceExpPoints(int rewardAmount)
    {
        experiencePoint -= rewardAmount;
        experienceText.text = experiencePoint.ToString(); 
    }
    void ReduceGold(int rewardAmount)
    {
        gold -= rewardAmount;
        goldText.text = gold.ToString();
    }
    public void KillBarbarian()
    {
        GameEvents.TriggerEvent("EnemyKilled", 1);
    }
    public void KillArcher()
    {
        GameEvents.TriggerEvent("EnemyKilled", 2);
    }
    public void PickRose()
    {
        GameEvents.TriggerEvent("ItemPicked", 1);
    }
    public void PickWood()
    {
        GameEvents.TriggerEvent("ItemPicked", 2);
    }

}
