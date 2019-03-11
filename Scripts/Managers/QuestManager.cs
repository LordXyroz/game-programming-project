using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestManager : MonoBehaviour {

    public DialogManager dialogManager;

    public List<BaseQuest> completedQuests;
    public List<BaseQuest> activeQuests;


    public void Update()
    {
        // Loops through every active quest the player has, and updates it's progression
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            activeQuests[i].CheckProgress(i, transform.parent);
            if (activeQuests[i].state == BaseQuest.QuestState.Completed)
                CompletedQuest(activeQuests[i]);
        }
    }

    public bool CheckCompleted(BaseQuest quest)
    {
        // Checks if the player has completed a specific quest
        foreach (var temp in completedQuests)
        {
            if (temp.questID == quest.questID)
                return true;
        }
        return false;
    }

    public bool CheckActive(BaseQuest quest)
    {
        // Checks if the player has the specific quest active
        foreach (var temp in activeQuests)
        {
            if (temp.questID == quest.questID)
                return true;
        }
        return false;
    }

    public bool CheckPrerequisite(BaseQuest quest)
    {
        // Checks if a quests prerequisite has been met
        foreach (var temp in completedQuests)
        {
            if (temp.questID == quest.prerequisitQuest.questID)
                return true;
        }
        return false;
    }

    public void HandleQuest(BaseQuest quest)
    {
        // Tells dialogmanager that we want a "give quest" button active
        dialogManager.giveQuest.gameObject.SetActive(true);
        dialogManager.giveQuest.onClick.RemoveAllListeners();

        // Changes behavior of the button based on new or active quest
        if (!CheckActive(quest))
        {
            dialogManager.giveQuest.onClick.AddListener(() => GiveQuest(quest));
        }
        else
        { 
            dialogManager.giveQuest.onClick.AddListener(() => OnQuest(quest));
        }
    }

    public void HandleTrigger(GameObject obj)
    {
        // Handles a trigger when progressing a quest,
        // i.e. founc a location, killed a unit, looted item etc
        foreach (var quest in activeQuests)
        {
            quest.CheckTrigger(obj);
        }
    }

    private void GiveQuest(BaseQuest quest)
    {
        // Tells dialogmanager what buttons to have active, 
        // and what those buttons should do when it's a new quest
        dialogManager.giveQuest.gameObject.SetActive(false);
        dialogManager.accept.gameObject.SetActive(true);
        dialogManager.decline.gameObject.SetActive(true);

        dialogManager.accept.onClick.RemoveAllListeners();
        dialogManager.decline.onClick.RemoveAllListeners();

        dialogManager.accept.onClick.AddListener(() => AcceptQuest(quest));
        dialogManager.decline.onClick.AddListener(() => DeclineQuest(quest));

        dialogManager.ShowDialog(quest.questDescription);
    }

    private void OnQuest(BaseQuest quest)
    {
        // Checks if the quest can be handed in
        foreach (var temp in activeQuests)
        {
            if (temp.questID == quest.questID)
                if (temp.state == BaseQuest.QuestState.HandIn)
                {
                    // Quest can be handed in
                    HandInQuest(temp);
                    return;
                }
        }

        // Tells dialogmanager to show in progress text
        dialogManager.giveQuest.gameObject.SetActive(false);
        dialogManager.ShowDialog(quest.onQuestText);
    }

    private void AcceptQuest(BaseQuest quest)
    {
        // Disables buttons
        dialogManager.accept.gameObject.SetActive(false);
        dialogManager.decline.gameObject.SetActive(false);

        // Create a copy of the original quest, set it as child to the manager and set it to active
        var temp = Instantiate(quest, this.gameObject.transform);
        temp.state = BaseQuest.QuestState.Active;
        // Adds the new quest to list
        activeQuests.Add(temp);

        // Close dialog
        dialogManager.CloseBox();
    }

    private void DeclineQuest(BaseQuest quest)
    {
        // Disables buttons and closes dialog
        dialogManager.accept.gameObject.SetActive(false);
        dialogManager.decline.gameObject.SetActive(false);

        dialogManager.CloseBox();
    }

    private void CompletedQuest(BaseQuest quest)
    {
        // Changes a quests state to handin, and activates minimap marker of handin location
        quest.state = BaseQuest.QuestState.HandIn;
        quest.handInLocation.SetActive(true);

        dialogManager.ShowDialog(quest.completedQuestText);
    }

    private void HandInQuest(BaseQuest quest)
    {
        // Disables button
        dialogManager.giveQuest.gameObject.SetActive(false);

        // Moves the quest from active to completed list
        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        // Changes quest state, disables the object so Update() doesn't get called anymore
        // and disables minimap icon
        quest.state = BaseQuest.QuestState.Finished;
        quest.gameObject.SetActive(false);
        quest.handInLocation.SetActive(false);

        // Shows handin text
        dialogManager.ShowDialog(quest.handInQuestText);
    }
}
