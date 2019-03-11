using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverScript : MonoBehaviour {

    public BaseQuest quest;
    
    public void CheckQuest(GameObject player)
    {
        var questManager = player.GetComponentInChildren<QuestManager>();

        // Checks if the player has completed the quest, 
        // and tells the player's questmanager to handle this quest
        if (!questManager.CheckCompleted(quest))
        {
            if (quest.prerequisit)
            {
                if (!questManager.CheckPrerequisite(quest))
                    return;
            }
            questManager.HandleQuest(quest);
        }
    }
}
