using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationQuest : BaseQuest {

    [Header("Quest Location Lists")]
    public List<GameObject> questLocations = new List<GameObject>();
    public List<GameObject> completedLocations = new List<GameObject>();

    override public void CheckProgress(int id, Transform player)
    {
        // Calls baseclass's function
        base.CheckProgress(id, player);

        // Checks if all locations have been visited
        if (questLocations.Count == 0)
            if (state != QuestState.HandIn && state != QuestState.Finished)
                state = QuestState.Completed;

        // Loops through every quest location, update's minimap quest number,
        // rotates the number, and set's them to be active
        foreach (var location in questLocations)
        {
            location.GetComponentInChildren<TextMesh>().text = (id + 1).ToString();
            location.transform.rotation = player.rotation;
            location.SetActive(true);
        }
    }

    public override void CheckTrigger(GameObject obj)
    {
        // Loops through all locations
        for (int i = questLocations.Count - 1; i >= 0; i--)
        {
            // If the trigger is a location in the list
            if (questLocations[i] == obj)
            {
                // Disables the trigger and moves the location to completed list
                questLocations[i].SetActive(false);
                questLocations.RemoveAt(i);
                completedLocations.Add(obj);
                return;
            }

        }
    }
}
