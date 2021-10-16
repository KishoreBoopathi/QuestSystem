using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLists : MonoBehaviour
{
    List<Quest> questLists = new List<Quest>();
    List<GameObject> questSlot = new List<GameObject>();
    [SerializeField] Quest questPrefab;
    
    public void InitialiseAllQuest(Button populateButton)
    {
        questLists.Add(Instantiate(questPrefab).CreateQuest("Kill a bunch of barbarians", new QuestObjective(1, "Kill 3 Barbarians", ObjectiveType.Kill, 3), 100, "RewardGold", true));
        questLists.Add(Instantiate(questPrefab).CreateQuest("Kill a bunch of archers", new QuestObjective(2, "Kill 3 Archer", ObjectiveType.Kill, 3), true, 5.0f, 200, "RewardExperience", "ReduceExperience", false));
        questLists.Add(Instantiate(questPrefab).CreateQuest("Pickup a bunch of roses", new QuestObjective(1, "Collect 3 roses", ObjectiveType.Pickup, 3), 200,"RewardExperience", true));
        questLists.Add(Instantiate(questPrefab).CreateQuest("Pickup a bunch of woods", new QuestObjective(2, "Collect 3 woods", ObjectiveType.Pickup, 3), 100,"RewardGold", true));
        questLists.Add(Instantiate(questPrefab).CreateQuest("Kill a bunch of barbarians", new QuestObjective(1, "Kill 7 Barbarians", ObjectiveType.Kill, 7), true, 20.0f, 500,"RewardGold", "ReduceGold", false));
        questLists.Add(Instantiate(questPrefab).CreateQuest("Pickup a bunch of woods", new QuestObjective(2, "Collect 10 woods", ObjectiveType.Pickup, 10), true, 25.0f, 700,"RewardGold", "ReduceExperience", false));
        
        PopulateQuestList();
        if(populateButton != null)
        {
            populateButton.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate() 
    {
        DisplayQuestList();
    }

    void PopulateQuestList()
    {
        GameObject questImage = transform.GetChild(0).gameObject;
        for(int i = 0; i < questLists.Count; i++)
        {
            questSlot.Add(Instantiate(questImage, transform)); 
        }
         DisplayQuestList();
    }

    public void DisplayQuestList()
    {        
        for(int i = 0; i < questSlot.Count; i++)
        {
            questSlot[i].transform.GetChild(0).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().isQuestCompleted.ToString();       
            questSlot[i].transform.GetChild(1).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().questDescription;
            questSlot[i].transform.GetChild(2).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().rewardAmount.ToString();
            questSlot[i].transform.GetChild(3).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().objective.objectiveType.ToString();
            questSlot[i].transform.GetChild(4).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().objective.requiredAmount.ToString();
            questSlot[i].transform.GetChild(5).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().objective.aquiredAmount.ToString();
            questSlot[i].transform.GetChild(6).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().isQuestCompleted ? questLists[i].GetComponent<Quest>().isSuccess ? "Success" : "Failure" : "In Progress"; 
            questSlot[i].transform.GetChild(7).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().hasFailState ? Mathf.RoundToInt(questLists[i].GetComponent<Quest>().failTime).ToString() : " - ";
            questSlot[i].transform.GetChild(8).GetChild(0).GetComponent<Text>().text = questLists[i].GetComponent<Quest>().isQuestActive ? "Active" : "Inactive";
            questSlot[i].transform.GetChild(8).GetComponent<Button>().onClick.AddListener(questLists[i].ToggleQuestActiveStatus);
            questSlot[i].transform.GetChild(8).gameObject.SetActive(true);
        }
    }
}
