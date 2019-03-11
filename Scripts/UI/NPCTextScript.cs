using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextScript : MonoBehaviour {
    public string[] dialog;
    [SerializeField]
    private QuestGiverScript[] quests;

    // Use this for initialization
    void Start()
    {
        quests = gameObject.GetComponents<QuestGiverScript>();
    }


    public void Talk(GameObject player, DialogManager manager, GameObject optionsMenu)
    {
        // If the player is facing the NPC within a 90 degree angle
        float dot = Vector3.Dot(player.transform.forward, (transform.position - player.transform.position).normalized);
        if (dot > 0.7f)
        {
            // Checks if there isn't a dialog already active, and the optons menu isn't active
            if (!manager.dialogBox.activeSelf && !optionsMenu.activeSelf)
            {
                // Loops through every quest and checks if prerequisite, completed etc
                foreach (var quest in quests)
                {
                    quest.CheckQuest(player);
                }

                // Sets the first line of dialog and displays dialog
                manager.currentLine = 0;
                manager.ShowDialog(dialog);
            }
        }
    }
}
