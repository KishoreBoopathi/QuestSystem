using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct QuestObjective
{
    public ObjectiveType objectiveType;
    public string description;
    public bool isObjectiveCompleted;
    public int requiredAmount;
    public int aquiredAmount;
    public int id;

    public QuestObjective( int id, string description, ObjectiveType objectiveType, int requiredAmount)
    {
        this.objectiveType = objectiveType;
        this.id = id;
        this.description = description;
        this.requiredAmount = requiredAmount;
        this.aquiredAmount = 0;
        this.isObjectiveCompleted = false;
    }
}

public enum ObjectiveType
{
    Kill,
    Pickup
}
public class Quest : MonoBehaviour
{
    Coroutine failureCoroutine;
    Coroutine initialiseCoroutine;
    public string questDescription;
    public QuestObjective objective = new QuestObjective();
    public bool hasFailState = false;
    public float failTime = 0.0f;
    public int rewardAmount;
    public bool isQuestActive = false;
    public bool isQuestCompleted = false;
    public bool isSuccess;
    public string onSuccessAction;
    public string onFailureAction;
    bool isListening = false;

    private void Start() 
    {
        initialiseCoroutine = StartCoroutine(InitialiseQuest());
    }

    public Quest CreateQuest(string questDescription, QuestObjective objective, int rewardAmount, string onSuccessAction, bool activateQuest)
    {
        this.questDescription = questDescription;
        this.objective = objective;
        this.rewardAmount = rewardAmount;
        this.isQuestActive = activateQuest;
        this.onSuccessAction = onSuccessAction;
        
        return this;
    }

    public Quest CreateQuest(string questDescription, QuestObjective objective, bool hasFailState, float failTime, int rewardAmount, string onSuccessAction, string onFailureAction, bool activateQuest)
    {
        this.questDescription = questDescription;
        this.objective = objective;
        this.hasFailState = hasFailState;
        this.failTime = failTime;
        this.rewardAmount = rewardAmount;
        this.isQuestActive = activateQuest;
        this.onSuccessAction = onSuccessAction;
        this.onFailureAction = onFailureAction;

        return this;
    }
    public IEnumerator InitialiseQuest()
    {
        while(!isQuestActive)
        {
            yield return null;
        }

        StartListeners();

        if(hasFailState)
        {
            failureCoroutine = StartCoroutine(CheckQuestFailed());
        }
    }

    public void ToggleQuestActiveStatus()
    {
        if(!isQuestCompleted)
        {
            isQuestActive = !isQuestActive;
            if(isQuestActive)
                StartListeners();
            else
                StopListeners();
        }
            
    }

    public void CheckObjectiveCompleted()
    {
        if(objective.aquiredAmount >= objective.requiredAmount)
        {
            objective.isObjectiveCompleted = true;
            CheckQuestSucceeded();
        } 
    }

    public void CheckQuestSucceeded()
    {
        if(isQuestCompleted) return;

        if(!objective.isObjectiveCompleted)
        {
            return;
        }
        isQuestCompleted = true;
        isSuccess = true;
        Debug.Log("Quest Succeeded");
        SuccessEvent();
        this.gameObject.SetActive(false);
    }

    IEnumerator CheckQuestFailed()
    {
        while(failTime > 0)
        {
            if(isQuestActive)
                failTime -= Time.deltaTime;
            
            yield return null;    
        }
        failTime = 0;        
        if(!this.isQuestCompleted)
        {
            isQuestCompleted = true;
            isSuccess = false;
            Debug.Log("Quest Failed");
            FailureEvent();
            this.gameObject.SetActive(false);
        }
    }

    void StartListeners()
    {
        if(!isListening)
        {
            if(this.objective.objectiveType == ObjectiveType.Kill)
            {
                GameEvents.StartListening("EnemyKilled", EnemyKilled);
            }
            else if(this.objective.objectiveType == ObjectiveType.Pickup)
            {
                GameEvents.StartListening("ItemPicked", ItemPicked);
            }
            isListening = true;
        }
        
    }

    void StopListeners()
    {
        if(this.objective.objectiveType == ObjectiveType.Kill)
            GameEvents.StopListening("EnemyKilled", EnemyKilled);
        else if(this.objective.objectiveType == ObjectiveType.Pickup)
            GameEvents.StopListening("ItemPicked", ItemPicked);

        isListening = false;
    }

    void SuccessEvent()
    {
        GameEvents.TriggerEvent(onSuccessAction, rewardAmount);
    }

    void FailureEvent()
    {
        GameEvents.TriggerEvent(onFailureAction, rewardAmount);
    }

    public void EnemyKilled(int id)
    {
        if(isQuestCompleted) return;
        if(id == this.objective.id)
        {
           Debug.Log("Enemy "+ id +" killed");
           this.objective.aquiredAmount++;
           CheckObjectiveCompleted();
        }
    }

    public void ItemPicked(int id)
    {
        if(isQuestCompleted) return;
        if(id == this.objective.id)
        {
           Debug.Log("Item "+ id +" picked");
           this.objective.aquiredAmount++;
           CheckObjectiveCompleted();
        }
    }

    private void OnDisable() 
    {
        isQuestActive = false;
        StopListeners();
        
        //StopAllCoroutines();
    }
}

