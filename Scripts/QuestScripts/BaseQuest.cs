using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class BaseQuest : MonoBehaviour {
    // Base class for every quest type

    // Enum for progress in a quest
    public enum QuestState
    {
        Available,
        Active,
        Completed,
        HandIn,
        Finished
    }
    [Header("Quest info")]
    public int questID;
    public QuestState state = QuestState.Available;

    [Header("Quest text")]
    public string questTitle;
    public string[] questDescription;
    public string[] onQuestText;
    public string[] completedQuestText;
    public string[] handInQuestText;

    [Header("Quest Hand-in")]
    public GameObject handInLocation;

    [Header("Quest prerequisite")]
    public BaseQuest prerequisitQuest;
    public bool prerequisit;

    // Virtual functions to be overriden by derrived classes, or called on using base.funcname();
    virtual public void CheckProgress(int id, Transform player)
    {
        handInLocation.transform.rotation = player.rotation;
    }

    virtual public void CheckTrigger(GameObject obj)
    {
        return;
    }
}
